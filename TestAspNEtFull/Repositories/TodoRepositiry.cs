using System;
using MongoDB.Driver;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Providers;

namespace TestAspNEtFull.Repositories;

public interface ITodoRepository
{
    Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem);
    Task<List<TodoItemEntity>> GetAsync();
    Task<TodoItemEntity> GetAsync(string id);
    Task UpdateAsync(TodoItemEntity todoItem);
    Task DeleteAsync(string id);
}

public class TodoRepository : ITodoRepository
{
    private IMongoCollection<TodoItemEntity> _collection;

    public TodoRepository()
    {
        _collection = MobgoDBClient.Instance.GetCollection<TodoItemEntity>("TodoItems");
    }

    public async Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem)
    {
        await _collection.InsertOneAsync(todoItem);

        return todoItem;
    }

    public async Task DeleteAsync(string id)
    {
        await _collection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task<List<TodoItemEntity>> GetAsync()
    {
        return await _collection.Find(x => true).ToListAsync();
    }

    public async Task<TodoItemEntity> GetAsync(string id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(TodoItemEntity todoItem)
    {
        await _collection.ReplaceOneAsync(x => x.Id == todoItem.Id, todoItem);
    }
}

