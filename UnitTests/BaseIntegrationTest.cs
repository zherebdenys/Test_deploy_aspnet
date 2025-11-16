using Microsoft.Extensions.DependencyInjection;
using UnitTests;

public class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient Client;
    protected readonly IServiceProvider _services;

    public BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        _services = factory.Services;

        // Run seed before each test
        RunSeedAsync().Wait();
    }

    private async Task RunSeedAsync()
    {
        using var scope = _services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedTodosAsync();
    }
}