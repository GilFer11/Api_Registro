using ApiRegistrosDiarios.Data;
using ApiRegistrosDiarios.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<RegistroRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/registros", async (RegistroRepository repo) =>
{
    var data = await repo.GetAllAsync();
    return Results.Ok(data);
});

app.MapGet("/api/registros/{id:int}", async (int id, RegistroRepository repo) =>
{
    var item = await repo.GetByIdAsync(id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});

app.MapPost("/api/registros", async (RegistroDiario r, RegistroRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(r.Actividad) || string.IsNullOrWhiteSpace(r.Responsable))
        return Results.BadRequest("Actividad y Responsable son obligatorios.");

    var newId = await repo.CreateAsync(r);
    r.Id = newId;
    return Results.Created($"/api/registros/{newId}", r);
});

app.MapPut("/api/registros/{id:int}", async (int id, RegistroDiario r, RegistroRepository repo) =>
{
    var ok = await repo.UpdateAsync(id, r);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/api/registros/{id:int}", async (int id, RegistroRepository repo) =>
{
    var ok = await repo.DeleteAsync(id);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.Run();

