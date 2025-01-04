using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Restaurants;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.Restaurants;

public class RestaurantOwnerCreatedDomainEventHandler : INotificationHandler<RestaurantOwnerCreatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventLogService _eventLogService;
    private readonly EventBusConfiguration _eventBusConfig;

    public RestaurantOwnerCreatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IEventLogService eventLogService,
        IOptions<EventBusConfiguration> options)
    {
        _unitOfWork = unitOfWork;
        _eventLogService = eventLogService;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(RestaurantOwnerCreatedEvent notification, CancellationToken cancellationToken)
    {
        var restaurantCreatedEvent = new RestaurantOwnerCreatedDenormalizationEvent(notification.Id);

        await _eventLogService.AddEventAsync(
            restaurantCreatedEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
