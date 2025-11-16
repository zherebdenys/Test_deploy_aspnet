using System;
using MongoDB.Driver;
using TestAspNEtFull.Entities;

namespace UnitTests;


public class DatabaseSeeder
{
    private readonly IMongoDatabase _db;

    public DatabaseSeeder(IMongoDatabase db)
    {
        _db = db;
    }

    public async Task SeedTodosAsync()
    {
        var collection = _db.GetCollection<TodoItemEntity>("TodoItems");

        // Очистка перед seed
        await collection.DeleteManyAsync(FilterDefinition<TodoItemEntity>.Empty);

        // Seed дані
        var items = new List<TodoItemEntity>
        {
            new TodoItemEntity { Name = "Test Task 1", IsComplete = false },
            new TodoItemEntity { Name = "Test Task 2", IsComplete = true  },
            new TodoItemEntity { Name = "Test Task 3", IsComplete = false },
        };

        await collection.InsertManyAsync(items);
    }
}