using MongoDB.Driver;
using TestAspNEtFull.Entities;

namespace TestAspNEtFull.Repositories;

/// <summary>
/// Код TodoRepository - для роботи з даними в БД
/// </summary>

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
    private readonly IMongoCollection<TodoItemEntity> _collection;

    public TodoRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<TodoItemEntity>("TodoItems");
    }

    public async Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem)
    {
        await _collection.InsertOneAsync(todoItem);
        return todoItem;
    }

    public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);
    public async Task<List<TodoItemEntity>> GetAsync() => await _collection.Find(x => true).ToListAsync();
    public async Task<TodoItemEntity> GetAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    public async Task UpdateAsync(TodoItemEntity todoItem) => await _collection.ReplaceOneAsync(x => x.Id == todoItem.Id, todoItem);
}

