using MediatR;
using RestaurantManagement.Core.Domain.Contracts;
using RestaurantManagement.Core.Domain.Dtos;
using RestaurantManagement.Core.Domain.ValueObjects;
using RestaurantManagement.Core.Resources;

namespace RestaurantManagement.Core.Application.Command.Food;

public record CreateFoodCommand(
    string Name,
    decimal Price,
    string? Description,
    ICollection<Guid> FoodTypeIds)
    : IRequest<Result<EntityCreatedDto>>;

public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, Result<EntityCreatedDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodCommandRepository _foodRepository;
    private readonly IAuthService _authService;

    public CreateFoodCommandHandler(
        IUnitOfWork unitOfWork,
        IFoodCommandRepository foodRepository,
        IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _foodRepository = foodRepository;
        _authService = authService;
    }

    public async Task<Result<EntityCreatedDto>> Handle(
        CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var currentUserResult = _authService.CurrentUserId();
        if (currentUserResult.IsFailure)
            return currentUserResult.UnwrapError();

        var ownerId = currentUserResult.Unwrap();

        if (await _foodRepository.ExistsWithName(ownerId, request.Name, cancellationToken: cancellationToken))
            return new Error(CommonResource.App_CategoryAlreadyExists);

        var foodResult = FoodSpecification.Validate(request.Name, request.Price, request.Description)
            .And(new Domain.Models.FoodAggregate.Food(
                new FoodSpecification(request.Name, request.Price, request.Description),
                ownerId,
                request.FoodTypeIds));

        if (foodResult.IsFailure)
            return foodResult.UnwrapError();

        await _foodRepository.AddAsync(foodResult.Unwrap(), cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return EntityCreatedDto.From(foodResult.Unwrap().Id);
    }
}
