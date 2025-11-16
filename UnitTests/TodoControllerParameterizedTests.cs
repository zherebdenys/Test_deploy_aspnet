using System;
using System.Net.Http.Json;
using TestAspNEtFull.Entities;

namespace UnitTests;

public class TodoControllerParameterizedTests : BaseIntegrationTest
{
    public TodoControllerParameterizedTests(CustomWebApplicationFactory factory) : base(factory) { }

    public static IEnumerable<object[]> ValidIds()
    {
        var ids = new List<string>();
        using var client = new CustomWebApplicationFactory().CreateClient();
        // Отримуємо всі seeded items і повертаємо їхні ID
        var items = client.GetFromJsonAsync<List<TodoItemEntity>>("/api/todo").Result;

        foreach (var i in items)
        {
            yield return new object[] { i.Id };
        }
    }

    [Theory]
    [MemberData(nameof(ValidIds))]
    public async Task GetTodoById_Parameterized_Success(string id)
    {
        var response = await Client.GetAsync($"/api/todo/{id}");

        response.EnsureSuccessStatusCode();

        var item = await response.Content.ReadFromJsonAsync<TodoItemEntity>();

        Assert.NotNull(item);
        Assert.Equal(id, item.Id);
    }
}