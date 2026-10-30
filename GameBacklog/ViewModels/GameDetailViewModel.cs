using System.Collections.ObjectModel;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class GameDetailViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private Game? game;
    private string newAchievementName = string.Empty;
    private string newAchievementDescription = string.Empty;
    private string playtimeToAdd = string.Empty;

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

    public ObservableCollection<Achievement> UnlockedAchievements { get; } = new();

    public ObservableCollection<Achievement> LockedAchievements { get; } = new();

    public string PlaytimeToAdd
    {
        get => playtimeToAdd;
        set => SetProperty(ref playtimeToAdd, value);
    }

    public string NewAchievementName
    {
        get => newAchievementName;
        set => SetProperty(ref newAchievementName, value);
    }

    public string NewAchievementDescription
    {
        get => newAchievementDescription;
        set => SetProperty(ref newAchievementDescription, value);
    }

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
        RefreshAchievements();
    }

    public async Task LogPlaytimeAsync()
    {
        if (Game is null || !double.TryParse(PlaytimeToAdd, out var hours) || hours <= 0)
            return;

        Game.PlaytimeHours += hours;
        PlaytimeToAdd = string.Empty;
        await repository.SaveAsync();
    }

    public async Task DeleteAsync()
    {
        if (Game is not null)
            await repository.RemoveAsync(Game);
    }

    public async Task AddAchievementAsync()
    {
        if (Game is null || string.IsNullOrWhiteSpace(NewAchievementName))
            return;

        Game.Achievements.Add(new Achievement
        {
            Name = NewAchievementName.Trim(),
            Description = NewAchievementDescription.Trim()
        });

        NewAchievementName = string.Empty;
        NewAchievementDescription = string.Empty;
        Game.NotifyAchievementsChanged();
        RefreshAchievements();
        await repository.SaveAsync();
    }

    public async Task ToggleAchievementAsync(Achievement achievement)
    {
        achievement.UnlockedAt = achievement.IsUnlocked ? null : DateTime.Now;
        Game?.NotifyAchievementsChanged();
        RefreshAchievements();
        await repository.SaveAsync();
    }

    private void RefreshAchievements()
    {
        UnlockedAchievements.Clear();
        LockedAchievements.Clear();

        if (Game is null)
            return;

        foreach (var achievement in Game.Achievements.OrderByDescending(a => a.UnlockedAt))
        {
            if (achievement.IsUnlocked)
                UnlockedAchievements.Add(achievement);
            else
                LockedAchievements.Add(achievement);
        }
    }
}
