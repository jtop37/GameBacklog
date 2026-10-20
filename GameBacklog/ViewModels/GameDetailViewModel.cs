using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class GameDetailViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private Game? game;

    public GameDetailViewModel(GameRepository repository)
    {
        this.repository = repository;
    }

    public Game? Game
    {
        get => game;
        private set => SetProperty(ref game, value);
    }

    public async Task LoadAsync(Guid id)
    {
        await repository.LoadAsync();
        Game = repository.Find(id);
    }
}
