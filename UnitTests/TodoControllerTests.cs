using System;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Models;

namespace UnitTests;


//public class TodoControllerTests
//: BaseIntegrationTest
//{
//    public TodoControllerTests(CustomWebApplicationFactory factory) : base(factory) { }

//    [Fact]
//    public async Task CreateTodo_ShouldReturnCreatedItem()
//    {
//        // Arrange
//        var newTodo = new TodoItem
//        {
//            Name = "Test Item",
//            IsComplete = false
//        };

//        // Act
//        var response = await Client.PostAsJsonAsync("/api/todo", newTodo);

//        // Assert
//        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

//        var created = await response.Content.ReadFromJsonAsync<TodoItemEntity>();

//        Assert.NotNull(created);
//        Assert.Equal("Test Item", created.Name);
//        Assert.False(created.IsComplete);
//        Assert.False(string.IsNullOrWhiteSpace(created.Id));
//    }

//    [Fact]
//    public async Task GetTodos_ReturnsSeededItems()
//    {
//        var response = await Client.GetAsync("/api/todo");

//        response.EnsureSuccessStatusCode();

//        var items = await response.Content.ReadFromJsonAsync<List<TodoItemEntity>>();

//        Assert.Equal(3, items.Count);
//        Assert.Contains(items, x => x.Name == "Test Task 1");
//    }

//    [Fact]
//    public async Task CreateTodo_AddsNewItemToDatabase()
//    {
//        var newTodo = new TodoItem
//        {
//            Name = "New Integration Task",
//            IsComplete = false
//        };

//        var response = await Client.PostAsJsonAsync("/api/todo", newTodo);
//        response.EnsureSuccessStatusCode();

//        // Так як ми використовуємо DI контейнер, можемо отримати IMongoDatabase для перевірки
//        var db = _services.GetRequiredService<IMongoDatabase>();
//        // Тепер можемо користуватися db для перевірки
//        var items = await db.GetCollection<TodoItemEntity>("TodoItems")
//                            .Find(x => true)
//                            .ToListAsync();

//        // Після створення нового елемента, загальна кількість має бути 4 (3 з seed + 1 новий)
//        Assert.Equal(4, items.Count);
//    }
//}

public class TodoControllerTests : BaseIntegrationTest
{
    public TodoControllerTests(CustomWebApplicationFactory factory) : base(factory) { }

    // ------------------------------------------------------------
    // GET: /api/todo
    // ------------------------------------------------------------
    [Fact]
    public async Task GetTodos_ReturnsSeededItems()
    {
        var response = await Client.GetAsync("/api/todo");

        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<List<TodoItemEntity>>();

        Assert.NotNull(items);
        Assert.Equal(3, items.Count);  // 3 seeded items
        Assert.Contains(items, x => x.Name == "Test Task 1");
    }

    // ------------------------------------------------------------
    // GET: /api/todo/{id}
    // ------------------------------------------------------------
    [Fact]
    public async Task GetTodoById_ReturnsCorrectItem()
    {
        // get seeded items first
        var list = await Client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo");
        var firstId = list[0].Id;

        var response = await Client.GetAsync($"/api/todo/{firstId}");

        response.EnsureSuccessStatusCode();

        var item = await response.Content.ReadFromJsonAsync<TodoItemEntity>();

        Assert.NotNull(item);
        Assert.Equal(firstId, item.Id);
    }

    [Fact]
    public async Task GetTodoById_ReturnsNotFound_ForInvalidId()
    {
        var response = await Client.GetAsync("/api/todo/6748a68293acd000228f0000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ------------------------------------------------------------
    // POST: /api/todo
    // ------------------------------------------------------------
    [Fact]
    public async Task CreateTodo_ReturnsCreatedItem()
    {
        var newTodo = new TodoItem
        {
            Name = "Integration Test Create",
            IsComplete = false
        };

        var response = await Client.PostAsJsonAsync("/api/todo", newTodo);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<TodoItemEntity>();

        Assert.NotNull(created);
        Assert.Equal("Integration Test Create", created.Name);
        Assert.False(created.IsComplete);
        Assert.False(string.IsNullOrWhiteSpace(created.Id));

        // verify DB count
        var list = await Client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo");
        Assert.Equal(4, list.Count); // 3 seeded + 1 new
    }

    // ------------------------------------------------------------
    // PUT: /api/todo/{id}
    // ------------------------------------------------------------
    [Fact]
    public async Task UpdateTodo_ReturnsNoContent_AndUpdatesInDatabase()
    {
        // get seeded item
        var list = await Client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo");
        var item = list.First();

        var updated = new TodoItem
        {
            Id = 0, // ignored
            Name = "Updated Name",
            IsComplete = true
        };

        var response = await Client.PutAsJsonAsync($"/api/todo/{item.Id}", updated);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedFromDb = await Client.GetFromJsonAsync<TodoItemEntity>($"/api/todo/{item.Id}");

        Assert.Equal("Updated Name", updatedFromDb.Name);
        Assert.True(updatedFromDb.IsComplete);
    }

    [Fact]
    public async Task UpdateTodo_ReturnsNotFound_ForInvalidId()
    {
        var updated = new TodoItem
        {
            Name = "Updated Name",
            IsComplete = true
        };

        var response = await Client.PutAsJsonAsync("/api/todo/6748a68293acd000228f0000", updated);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // ------------------------------------------------------------
    // DELETE: /api/todo/{id}
    // ------------------------------------------------------------
    [Fact]
    public async Task DeleteTodo_RemovesItemFromDatabase()
    {
        var list = await Client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo");
        var item = list.First();

        var response = await Client.DeleteAsync($"/api/todo/{item.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var allAfter = await Client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo");

        Assert.Equal(2, allAfter.Count); // 3 seeded - 1 deleted
    }

    [Fact]
    public async Task DeleteTodo_ReturnsNotFound_ForInvalidId()
    {
        var response = await Client.DeleteAsync("/api/todo/6748a68293acd000228f0000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}