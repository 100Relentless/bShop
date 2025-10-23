namespace eShop.DigitalAthletes.API.IntegrationEvents.Events;

public record OrderStatusChangedToPaidIntegrationEvent(
    int OrderId,
    string BuyerId,
    IEnumerable<OrderStockItem> OrderStockItems) : IntegrationEvent;

public record OrderStockItem(int ProductId, int Units);
