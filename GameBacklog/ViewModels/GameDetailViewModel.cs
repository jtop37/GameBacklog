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

    public IReadOnlyList<string> StatusOptions => GameStatusExtensions.Labels;

    public int StatusIndex
    {
        get => Game is null ? 0 : (int)Game.Status;
        set
        {
            if (Game is null || value < 0 || (int)Game.Status == value)
                return;

            Game.Status = (GameStatus)value;
            OnPropertyChanged();
            _ = repository.SaveAsync();
        }
    }

    public double RatingValue
    {
        get => Game?.Rating ?? 0;
        set
        {
            if (Game is null || Game.Rating == (int)value)
                return;

            Game.Rating = (int)value;
            OnPropertyChanged();
            _ = repository.SaveAsync();
        }
    }

    public async Task LoadAsync(Guid id)
    {
        await repository.LoadAsync();
        Game = repository.Find(id);
        OnPropertyChanged(nameof(StatusIndex));
        OnPropertyChanged(nameof(RatingValue));
    }
}
