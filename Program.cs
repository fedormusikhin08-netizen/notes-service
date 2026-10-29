using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

var app = builder.Build();

var notes = new List<Note>
{
    new(1, "Покупки", "Молоко, хлеб, кофе", new DateTime(2026, 10, 27)),
    new(2, "Идеи для проекта", "Сделать API заметок и собрать его в Docker", new DateTime(2026, 10, 28)),
    new(3, "Учёба", "Дочитать конспект по ASP.NET Core", new DateTime(2026, 10, 29))
};

app.MapGet("/health", () => "OK");

app.MapGet("/notes", () => notes);

app.MapGet("/notes/{id}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);
    return note is null ? Results.NotFound() : Results.Ok(note);
});

app.Run();

public record Note(int Id, string Title, string Text, DateTime CreatedAt);
