using GameStoreServer.Models;

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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("http://localhost:5144")
           .AllowAnyHeader()
           .AllowAnyMethod();
}));

var app = builder.Build();

app.UseCors();

var group = app.MapGroup("/games")
                .WithParameterValidation();

//GET /games
group.MapGet("/", () => games);

//GET /games/{id}
group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(game);
})
.WithName("GetGame");

// POST /games
group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(x => x.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
});


//PUT /games/{id]
group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is null)
    {
        updatedGame.Id = id;
        games.Add(updatedGame);

        return Results.CreatedAtRoute("GetGame", new { id = updatedGame.Id }, updatedGame);
    }

    game.Name = updatedGame.Name;
    game.Price = updatedGame.Price;
    game.Genre = updatedGame.Genre;
    game.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
});

//Delete /games/{id]
group.MapDelete("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);

    if (game is null)
    {
        return Results.NotFound();

    }

    games.Remove(game);

    return Results.NoContent();
});


app.Run();
