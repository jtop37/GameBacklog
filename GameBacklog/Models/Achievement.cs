using System.Text.Json.Serialization;
using GameBacklog.Common;

namespace GameBacklog.Models;

public class Achievement : ObservableObject
{
    private string name = string.Empty;
    private string description = string.Empty;
    private DateTime? unlockedAt;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    public DateTime? UnlockedAt
    {
        get => unlockedAt;
        set
        {
            if (SetProperty(ref unlockedAt, value))
            {
                OnPropertyChanged(nameof(IsUnlocked));
                OnPropertyChanged(nameof(UnlockedText));
            }
        }
    }

    [JsonIgnore]
    public bool IsUnlocked => UnlockedAt.HasValue;

    [JsonIgnore]
    public string UnlockedText => UnlockedAt is { } date ? $"Unlocked {date:d MMM yyyy}" : "Locked";
}
