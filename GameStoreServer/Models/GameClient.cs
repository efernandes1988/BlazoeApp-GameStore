namespace GameStoreServer.Models;

public static class GameClient
{
    private static readonly List<Game> games = new()
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

    public static Game[] GetGames()
    {
        return games.ToArray();
    }

    public static void AddGame(Game game)
    {
        game.Id = games.Max(x => x.Id) + 1;
        games.Add(game);
    }

    public static Game GetGame(int id)
    {
        return games.Find(game => game.Id == id) ?? throw new Exception("Could not find game!");
    }

    public static void UpdateGame(Game game)
    {
        Game existingGame = GetGame(game.Id);
        existingGame.Name = game.Name;
        existingGame.Genre = game.Genre;
        existingGame.Price = game.Price;
        existingGame.ReleaseDate = game.ReleaseDate;
    }

    public static void DeleteGame(int id)
    {
        Game game = GetGame(id);
        games.Remove(game);
    }
}
