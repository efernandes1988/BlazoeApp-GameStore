﻿using System.Net.Http.Json;

namespace GameStore.Models;

public class GameClient
{
    private readonly HttpClient _httpClient;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Game[]?> GetGamesAsync(string? filter)
    {
        return await _httpClient.GetFromJsonAsync<Game[]>($"games?filter={filter}");
    }

    public async Task AddGameAsync(Game game)
    {

        await _httpClient.PostAsJsonAsync("games", game);

    }

    public async Task<Game?> GetGameAsync(int id)
    {

        return await _httpClient.GetFromJsonAsync<Game?>($"games/{id}") ?? throw new Exception("Could not find game!");

    }

    public async Task UpdateGameAsync(Game game)
    {

        await _httpClient.PutAsJsonAsync($"games/{game.Id}", game);

    }

    public async Task DeleteGameAsync(int id)
    {

        await _httpClient.DeleteAsync($"games/{id}");

    }
}
