using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoListContext>(
     options => options.UseSqlServer("name=ToDoDB:DefaultConnection"));

builder.Services.AddScoped<ToDoListContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("myAppCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});
var app = builder.Build();

app.MapGet("/items", async (ToDoListContext db) =>
    await db.Items.ToListAsync());

app.MapPost("/items", async (string name, ToDoListContext dbContext) =>
{
    Item newItem =new Item(){Name=name, IsComplete=0};
    dbContext.Items.Add(newItem);
    await dbContext.SaveChangesAsync();
    return newItem;
});

app.MapPut("/items/{id}", async (ToDoListContext db, int id, short isComplete) =>
{
    var todo = await db.Items.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.IsComplete = isComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (ToDoListContext db, int id) =>
{
    if (await db.Items.FindAsync(id) is Item todo)
    {
        Console.WriteLine(id);
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.UseCors("myAppCors");
app.Run();
