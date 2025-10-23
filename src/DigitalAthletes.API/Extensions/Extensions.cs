public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Avoid loading full database config and migrations if startup
        // is being invoked from build-time OpenAPI generation
        if (builder.Environment.IsBuild())
        {
            builder.Services.AddDbContext<DigitalAthletesContext>();
            return;
        }

        builder.AddNpgsqlDbContext<DigitalAthletesContext>("digitalathletesdb");

        // Add database migration and seeding
        builder.Services.AddMigration<DigitalAthletesContext, DigitalAthletesContextSeed>();

        // Add the integration services that consume the DbContext
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<DigitalAthletesContext>>();

        builder.Services.AddTransient<IDigitalAthletesIntegrationEventService, DigitalAthletesIntegrationEventService>();

        // Add event bus with subscriptions
        builder.AddRabbitMqEventBus("eventbus")
               .AddSubscription<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();

        // Configure options
        builder.Services.AddOptions<DigitalAthletesOptions>()
            .BindConfiguration(nameof(DigitalAthletesOptions));

        // Add Digital Athletes services
        builder.Services.AddScoped<DigitalAthletesServices>();
    }
}
