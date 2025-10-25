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
    private readonly TodoItemService _service;
    private static List<TodoItem> todos = new()
    {
        new TodoItem { Id = 1, Name = "Вивчити ASP.NET Core Web API", IsComplete = true },
        new TodoItem { Id = 2, Name = "Створити власний ToDo контролер", IsComplete = false }
    };

    public TodoController()
    {
        _service = new();
    }

    // GET: api/values
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAsync());
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var todo = await _service.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        return Ok(todo);
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]TodoItem newTodo)
    {
        var item = await _service.CreateAsync(new TodoItemEntity()
        {
            Name = newTodo.Name,
            IsComplete = newTodo.IsComplete
        });

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] TodoItem updatedTodo)
    {

        var todo = await _service.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        var newUpdatedEntity = new TodoItemEntity()
        {
            Id = id,
            Name = updatedTodo.Name,
            IsComplete = updatedTodo.IsComplete
        };

        await _service.UpdateAsync(newUpdatedEntity);

        return NoContent();
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var todo = await _service.GetAsync(id);

        if (todo == null)
            return NotFound($"Завдання з ID={id} не знайдено.");

        await _service.DeleteAsync(id);

        return NoContent();
    }
}

