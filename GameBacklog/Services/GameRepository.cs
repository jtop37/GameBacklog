using System.Collections.ObjectModel;
using System.Text.Json;
using GameBacklog.Models;

namespace GameBacklog.Services;

public class GameRepository
{
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "games.json");
    private readonly SemaphoreSlim gate = new(1, 1);
    private bool loaded;

    public ObservableCollection<Game> Games { get; } = new();

    public async Task LoadAsync()
    {
        await gate.WaitAsync();
        try
        {
            if (loaded)
                return;

            if (File.Exists(filePath))
            {
                await using var stream = File.OpenRead(filePath);
                var games = await JsonSerializer.DeserializeAsync<List<Game>>(stream, SerializerOptions);
                foreach (var game in games ?? new List<Game>())
                    Games.Add(game);
            }

            loaded = true;
        }
        finally
        {
            gate.Release();
        }
    }

    public async Task SaveAsync()
    {
        await gate.WaitAsync();
        try
        {
            await using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, Games, SerializerOptions);
        }
        finally
        {
            gate.Release();
        }
    }

    public Game? Find(Guid id) => Games.FirstOrDefault(game => game.Id == id);

    public async Task AddAsync(Game game)
    {
        Games.Add(game);
        await SaveAsync();
    }

    public async Task RemoveAsync(Game game)
    {
        Games.Remove(game);
        await SaveAsync();
    }
}
