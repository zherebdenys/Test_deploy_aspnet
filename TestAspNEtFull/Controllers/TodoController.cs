using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestAspNEtFull.Entities;
using TestAspNEtFull.Models;
using TestAspNEtFull.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAspNEtFull.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _todoService.GetAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var todo = await _todoService.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        return Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]TodoItem newTodo)
    {
        var item = await _todoService.CreateAsync(new TodoItemEntity()
        {
            Name = newTodo.Name,
            IsComplete = newTodo.IsComplete
        });

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] TodoItem updatedTodo)
    {

        var todo = await _todoService.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        var newUpdatedEntity = new TodoItemEntity()
        {
            Id = id,
            Name = updatedTodo.Name,
            IsComplete = updatedTodo.IsComplete
        };

        await _todoService.UpdateAsync(newUpdatedEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var todo = await _todoService.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        await _todoService.DeleteAsync(id);

        return NoContent();
    }
}

