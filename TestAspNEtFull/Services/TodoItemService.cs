using System;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Repositories;

namespace TestAspNEtFull.Services;

public interface ITodoService
{
    Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem);
    Task<List<TodoItemEntity>> GetAsync();
    Task<TodoItemEntity> GetAsync(string id);
    Task UpdateAsync(TodoItemEntity todoItem);
    Task DeleteAsync(string id);
}

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepository;

    public TodoService(ITodoRepository todoRepository)
	{
        _todoRepository = todoRepository;
    }

    public async Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem)
    {
        return await _todoRepository.CreateAsync(todoItem);
    }

    public async Task DeleteAsync(string id)
    {
        await _todoRepository.DeleteAsync(id);
    }

    public async Task<List<TodoItemEntity>> GetAsync()
    {
        return await _todoRepository.GetAsync();
    }

    public async Task<TodoItemEntity> GetAsync(string id)
    {
        return await _todoRepository.GetAsync(id);
    }

    public async Task UpdateAsync(TodoItemEntity todoItem)
    {
        await _todoRepository.UpdateAsync(todoItem);
    }
}

