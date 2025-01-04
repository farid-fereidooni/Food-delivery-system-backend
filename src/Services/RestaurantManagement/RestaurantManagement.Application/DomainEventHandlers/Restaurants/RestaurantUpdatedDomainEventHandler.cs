using EventBus.Core;
using MediatR;
using Microsoft.Extensions.Options;
using RestaurantManagement.Application.DenormalizationEvents.Restaurants;
using RestaurantManagement.Domain.Contracts;
using RestaurantManagement.Domain.DomainEvents.Restaurants;
using RestaurantManagement.Domain.Dtos;

namespace RestaurantManagement.Application.DomainEventHandlers.Restaurants;

public class RestaurantUpdatedDomainEventHandler : INotificationHandler<RestaurantUpdatedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventLogService _eventLogService;
    private readonly EventBusConfiguration _eventBusConfig;

    public RestaurantUpdatedDomainEventHandler(
        IUnitOfWork unitOfWork,
        IEventLogService eventLogService,
        IOptions<EventBusConfiguration> options)
    {
        _unitOfWork = unitOfWork;
        _eventLogService = eventLogService;
        _eventBusConfig = options.Value;
    }

    public async Task Handle(RestaurantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var restaurantUpdatedEvent = new RestaurantUpdatedDenormalizationEvent(
            notification.Id,
            notification.Name,
            notification.Street,
            notification.City,
            notification.State,
            notification.ZipCode);

        await _eventLogService.AddEventAsync(
            restaurantUpdatedEvent,
            _eventBusConfig.DenormalizationBroker,
            _unitOfWork.CurrentTransactionId ?? throw new Exception("Transaction has not been started"),
            cancellationToken);
    }
}
