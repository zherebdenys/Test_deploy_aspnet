using System;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Repositories;

namespace TestAspNEtFull.Services;

/// <summary>
/// Код TodoService - для роботи даними отриманими з БД
/// </summary>

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

    public async Task<TodoItemEntity> CreateAsync(TodoItemEntity todoItem) => await _todoRepository.CreateAsync(todoItem);
    public async Task DeleteAsync(string id) => await _todoRepository.DeleteAsync(id);
    public async Task<List<TodoItemEntity>> GetAsync() => await _todoRepository.GetAsync();
    public async Task<TodoItemEntity> GetAsync(string id) => await _todoRepository.GetAsync(id);
    public async Task UpdateAsync(TodoItemEntity todoItem) => await _todoRepository.UpdateAsync(todoItem);
}

