namespace GameBacklog.Models;

public enum GameStatus
{
    ToBePlayed,
    Playing,
    Completed,
    OnHold,
    Dropped
}

public static class GameStatusExtensions
{
    public static IReadOnlyList<string> Labels { get; } =
        Enum.GetValues<GameStatus>().Select(status => status.Label()).ToList();

    public static string Label(this GameStatus status) => status switch
    {
        GameStatus.ToBePlayed => "To be played",
        GameStatus.Playing => "Playing",
        GameStatus.Completed => "Completed",
        GameStatus.OnHold => "On hold",
        GameStatus.Dropped => "Dropped",
        _ => status.ToString()
    };
}
