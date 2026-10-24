namespace GameBacklog.Models;

public enum GameSortMode
{
    Name,
    Rating,
    Year,
    Playtime,
    RecentlyAdded
}

public static class GameSortModeExtensions
{
    public static IReadOnlyList<string> Labels { get; } = new[]
    {
        "Name",
        "Rating",
        "Release year",
        "Playtime",
        "Recently added"
    };

    public static IEnumerable<Game> Apply(this GameSortMode mode, IEnumerable<Game> games) => mode switch
    {
        GameSortMode.Rating => games.OrderByDescending(game => game.Rating).ThenBy(game => game.Title),
        GameSortMode.Year => games.OrderByDescending(game => game.Year).ThenBy(game => game.Title),
        GameSortMode.Playtime => games.OrderByDescending(game => game.PlaytimeHours).ThenBy(game => game.Title),
        GameSortMode.RecentlyAdded => games.OrderByDescending(game => game.AddedAt),
        _ => games.OrderBy(game => game.Title, StringComparer.CurrentCultureIgnoreCase)
    };
}
