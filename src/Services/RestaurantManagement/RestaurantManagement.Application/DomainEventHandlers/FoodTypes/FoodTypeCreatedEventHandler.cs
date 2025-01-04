using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.FoodTypes;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.FoodTypes;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.FoodTypes;

public class FoodTypeCreatedEventHandler : INotificationHandler<FoodTypeCreatedEvent>
{
    private readonly IEventLogService _eventLogService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EventBusConfiguration _eventBusConfig;

    public FoodTypeCreatedEventHandler(
        IEventLogService eventLogService,
        IUnitOfWork unitOfWork,
        IOptions<EventBusConfiguration> options)
    {
        _eventLogService = eventLogService;
        _unitOfWork = unitOfWork;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(FoodTypeCreatedEvent notification, CancellationToken cancellationToken)
    {
        var foodTypeCreatedDenormalizationEvent = new FoodTypeCreatedDenormalizationEvent()
        {
            Id = notification.Id,
            Name = notification.Name,
        };

        await _eventLogService.AddEventAsync(
            foodTypeCreatedDenormalizationEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
