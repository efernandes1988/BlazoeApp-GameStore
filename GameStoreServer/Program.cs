using GameStoreServer.Data;
using GameStoreServer.Models;
using Microsoft.EntityFrameworkCore;

/*
List<Game> games = new()
{

    new Game()
    {
        Id = 1,
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99M,
        ReleaseDate = new DateTime(1991, 2, 1)
    },
    new Game()
    {
        Id = 2,
        Name = "Fifa 23",
        Genre = "Football",
        Price = 39.99M,
        ReleaseDate = new DateTime(2022, 9, 27)
    },
    new Game()
    {
        Id = 3,
        Name = "Last Of Us 2",
        Genre = "RPG",
        Price = 69.99M,
        ReleaseDate = new DateTime(2020, 2, 1)
    }

};
*/
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("http://localhost:5144")
           .AllowAnyHeader()
           .AllowAnyMethod();
}));

var connString = builder.Configuration.GetConnectionString("GameStoreContext");
builder.Services.AddSqlServer<GameStoreContext>(connString);
/* 
 * Para a password é usada a funcionalidade Secret Manager
 * Para isso abrir terminal, navegar para a pasta do projeto, e fazer a seguinte instrução dotnet user-secrets init
 * Será adicionado ao projeto ao GameStoreServer.csproj
*/

var app = builder.Build();

app.UseCors();

var group = app.MapGroup("/games")
                .WithParameterValidation();

//GET /games
group.MapGet("/", async (string? filter, GameStoreContext context) =>
{

    var games = context.Games.AsNoTracking();

    if (filter is not null)
    {
        games = games.Where(game => game.Name.Contains(filter) || game.Genre.Contains(filter));
    }

    return await games.ToListAsync();
});

//GET /games/{id}
group.MapGet("/{id}", async (int id, GameStoreContext context) =>
{
    Game? game = await context.Games.FindAsync(id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName("GetGame");

// POST /games
group.MapPost("/", async (Game game, GameStoreContext context) =>
{
    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});


//PUT /games/{id]
group.MapPut("/{id}", async (int id, Game updatedGame, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id)
                .ExecuteUpdateAsync(updates =>
                updates.SetProperty(game => game.Name, updatedGame.Name)
                       .SetProperty(game => game.Genre, updatedGame.Genre)
                       .SetProperty(game => game.Price, updatedGame.Price)
                       .SetProperty(game => game.ReleaseDate, updatedGame.ReleaseDate)
                    );
    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

//Delete /games/{id]
group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
{
    var rowsAffected = await context.Games.Where(game => game.Id == id)
                .ExecuteDeleteAsync();

    return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});


app.Run();
