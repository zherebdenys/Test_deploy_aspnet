using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Mongo2Go;
using MongoDB.Driver;

namespace UnitTests;


public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private MongoDbRunner _runner;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            _runner = MongoDbRunner.Start();

            // Remove original MongoClient
            var descriptor = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IMongoClient));

            if (descriptor != null)
                services.Remove(descriptor);

            // Register test Mongo
            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(_runner.ConnectionString));

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("IntegrationTestsDb");
            });

            // Add seeder
            services.AddSingleton<DatabaseSeeder>();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _runner?.Dispose();
    }
}