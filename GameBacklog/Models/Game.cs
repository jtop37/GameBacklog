using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using GameBacklog.Common;

namespace GameBacklog.Models;

public class Game : ObservableObject
{
    private string title = string.Empty;
    private string platform = string.Empty;
    private int year;
    private string? imagePath;
    private GameStatus status = GameStatus.ToBePlayed;
    private int rating;
    private double playtimeHours;

    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime AddedAt { get; set; } = DateTime.Now;

    public ObservableCollection<Achievement> Achievements { get; set; } = new();

    public string Title
    {
        get => title;
        set
        {
            if (SetProperty(ref title, value))
                OnPropertyChanged(nameof(Initial));
        }
    }

    public string Platform
    {
        get => platform;
        set
        {
            if (SetProperty(ref platform, value))
                OnPropertyChanged(nameof(Subtitle));
        }
    }

    public int Year
    {
        get => year;
        set
        {
            if (SetProperty(ref year, value))
                OnPropertyChanged(nameof(Subtitle));
        }
    }

    public string? ImagePath
    {
        get => imagePath;
        set => SetProperty(ref imagePath, value);
    }

    public GameStatus Status
    {
        get => status;
        set
        {
            if (SetProperty(ref status, value))
                OnPropertyChanged(nameof(StatusText));
        }
    }

    public int Rating
    {
        get => rating;
        set
        {
            if (SetProperty(ref rating, value))
                OnPropertyChanged(nameof(RatingText));
        }
    }

    public double PlaytimeHours
    {
        get => playtimeHours;
        set
        {
            if (SetProperty(ref playtimeHours, value))
                OnPropertyChanged(nameof(PlaytimeText));
        }
    }

    [JsonIgnore]
    public string Initial => string.IsNullOrWhiteSpace(Title) ? "?" : Title.Trim()[..1].ToUpperInvariant();

    [JsonIgnore]
    public string Subtitle =>$"{Platform} · {Year}";

    [JsonIgnore]
    public string StatusText => Status.Label();

    [JsonIgnore]
    public string RatingText => Rating > 0 ? $"{Rating}/10" : "Not rated";

    [JsonIgnore]
    public string PlaytimeText => $"{PlaytimeHours:0.#} h";

    [JsonIgnore]
    public int UnlockedCount => Achievements.Count(achievement => achievement.IsUnlocked);

    [JsonIgnore]
    public string AchievementSummary => $"{UnlockedCount}/{Achievements.Count}";

    [JsonIgnore]
    public string CompletionText =>
        Achievements.Count == 0 ? "0%" : $"{100 * UnlockedCount / Achievements.Count}%";

    [JsonIgnore]
    public string AddedText => $"{AddedAt:d MMM yyyy}";

    public void NotifyAchievementsChanged()
    {
        OnPropertyChanged(nameof(UnlockedCount));
        OnPropertyChanged(nameof(AchievementSummary));
        OnPropertyChanged(nameof(CompletionText));
    }
}
