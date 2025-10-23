namespace eShop.DigitalAthletes.API.IntegrationEvents;

public interface IDigitalAthletesIntegrationEventService
{
    Task PublishThroughEventBusAsync(IntegrationEvent evt);
    Task SaveEventAndDigitalAthletesContextChangesAsync(IntegrationEvent evt);
}
