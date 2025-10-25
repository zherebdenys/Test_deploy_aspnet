using FluentValidation;
using FluentValidation.AspNetCore;
using TestAspNEtFull.Repositories;
using TestAspNEtFull.Services;
using TestAspNEtFull.Validatiors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Реєструємо всі валідатори автоматично
builder.Services.AddValidatorsFromAssemblyContaining<TodoItemValidator>();

builder.Services.AddTransient<ITodoRepository, TodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

