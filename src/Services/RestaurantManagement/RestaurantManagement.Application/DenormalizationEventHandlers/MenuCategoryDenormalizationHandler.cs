using EventBus.Core;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Application.DenormalizationEvents.MenuCategories;
using RestaurantManagement.Domain.Contracts.Query;
using RestaurantManagement.Domain.Models.Query;

namespace RestaurantManagement.Application.DenormalizationEventHandlers;

public class MenuCategoryDenormalizationHandler :
    IEventHandler<MenuCategoryCreatedDenormalizationEvent>,
    IEventHandler<MenuCategoryUpdatedDenormalizationEvent>
{
    private readonly IMenuCategoryQueryRepository _repository;
    private readonly ILogger<MenuCategoryDenormalizationHandler> _logger;

    public MenuCategoryDenormalizationHandler(
        IMenuCategoryQueryRepository repository, ILogger<MenuCategoryDenormalizationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(
        MenuCategoryCreatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(@event.MenuCategoryId, cancellationToken))
        {
            _logger.LogWarning("The event {eventName} seems to be already denormalized", @event.EventName);
            return;
        }

        await _repository.AddAsync(new MenuCategoryQuery
        {
            Id = @event.MenuCategoryId,
            OwnerId = @event.OwnerId,
            Name = @event.Name,
        }, cancellationToken);
    }

    public async Task HandleAsync(
        MenuCategoryUpdatedDenormalizationEvent @event, CancellationToken cancellationToken = default)
    {
        var category = await _repository.GetByIdAsync(@event.MenuCategoryId, cancellationToken);
        if (category == null)
        {
            _logger.LogWarning(
                "Denormalization event {eventName} Failed. Menu category with ID {id} not found",
                @event.EventName,
                @event.MenuCategoryId);
            throw new EventIgnoredException();
        }

        category.Name = @event.Name;

        await _repository.UpdateAsync(category, cancellationToken);
    }

}
