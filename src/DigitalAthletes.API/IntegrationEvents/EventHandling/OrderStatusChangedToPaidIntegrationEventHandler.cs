namespace eShop.DigitalAthletes.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler(
    DigitalAthletesContext context,
    ILogger<OrderStatusChangedToPaidIntegrationEventHandler> logger)
    : IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        logger.LogInformation(
            "Handling OrderStatusChangedToPaid integration event: {IntegrationEventId} - Order {OrderId} paid by buyer {BuyerId}",
            @event.Id, @event.OrderId, @event.BuyerId);

        // Grant access to digital athletes for this order
        foreach (var orderItem in @event.OrderStockItems)
        {
            // Check if this product is a digital athlete
            var athleteProduct = await context.DigitalAthleteProducts
                .FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);

            if (athleteProduct != null)
            {
                // Check if user already owns this athlete (shouldn't happen, but be safe)
                var existingOwnership = await context.UserOwnedAthletes
                    .FirstOrDefaultAsync(u => u.UserId == @event.BuyerId && u.AthleteProductId == orderItem.ProductId);

                if (existingOwnership == null)
                {
                    // Grant ownership
                    var ownership = new UserOwnedAthlete
                    {
                        UserId = @event.BuyerId,
                        AthleteProductId = orderItem.ProductId,
                        AcquiredDate = DateTime.UtcNow,
                        OrderId = @event.OrderId,
                        DownloadToken = Guid.NewGuid().ToString("N"),
                        DownloadCount = 0
                    };

                    context.UserOwnedAthletes.Add(ownership);

                    logger.LogInformation(
                        "Granted digital athlete {AthleteId} to user {UserId} from order {OrderId}",
                        orderItem.ProductId, @event.BuyerId, @event.OrderId);
                }
                else
                {
                    logger.LogWarning(
                        "User {UserId} already owns digital athlete {AthleteId}",
                        @event.BuyerId, orderItem.ProductId);
                }
            }
        }

        await context.SaveChangesAsync();
    }
}
