using System.Collections.ObjectModel;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public record StatusCount(string Label, int Count);

public class StatisticsViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private int ownedCount;
    private string totalPlaytime = "0 h";
    private string averageRating = "-";
    private string achievements = "0/0";
    private string mostPlayed = "-";

    public StatisticsViewModel(GameRepository repository)
    {
        this.repository = repository;
    }

    public ObservableCollection<StatusCount> StatusCounts { get; } = new();

    public int OwnedCount
    {
        get => ownedCount;
        private set => SetProperty(ref ownedCount, value);
    }

    public string TotalPlaytime
    {
        get => totalPlaytime;
        private set => SetProperty(ref totalPlaytime, value);
    }

    public string AverageRating
    {
        get => averageRating;
        private set => SetProperty(ref averageRating, value);
    }

    public string Achievements
    {
        get => achievements;
        private set => SetProperty(ref achievements, value);
    }

    public string MostPlayed
    {
        get => mostPlayed;
        private set => SetProperty(ref mostPlayed, value);
    }

    public async Task RefreshAsync()
    {
        await repository.LoadAsync();
        var games = repository.Games;

        OwnedCount = games.Count;
        TotalPlaytime = $"{games.Sum(game => game.PlaytimeHours):0.#} h";

        var rated = games.Where(game => game.Rating > 0).ToList();
        AverageRating = rated.Count == 0 ? "-" : $"{rated.Average(game => game.Rating):0.0}/10";

        Achievements = $"{games.Sum(game => game.UnlockedCount)}/{games.Sum(game => game.Achievements.Count)}";

        var top = games.OrderByDescending(game => game.PlaytimeHours).FirstOrDefault();
        MostPlayed = top is { PlaytimeHours: > 0 } ? $"{top.Title} ({top.PlaytimeText})" : "-";

        StatusCounts.Clear();
        foreach (var status in Enum.GetValues<GameStatus>())
            StatusCounts.Add(new StatusCount(status.Label(), games.Count(game => game.Status == status)));
    }
}
