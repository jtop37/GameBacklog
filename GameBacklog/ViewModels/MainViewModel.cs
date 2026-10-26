using System.Collections.ObjectModel;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private int sortIndex;
    private int filterIndex;

    public MainViewModel(GameRepository repository)
    {
        this.repository = repository;
    }

    public ObservableCollection<Game> Games { get; } = new();

    public IReadOnlyList<string> SortOptions => GameSortModeExtensions.Labels;

    public int SortIndex
    {
        get => sortIndex;
        set
        {
            if (value >= 0 && SetProperty(ref sortIndex, value))
                Refresh();
        }
    }

    public IReadOnlyList<string> FilterOptions { get; } =
        new[] { "All" }.Concat(GameStatusExtensions.Labels).ToList();

    public int FilterIndex
    {
        get => filterIndex;
        set
        {
            if (value >= 0 && SetProperty(ref filterIndex, value))
                Refresh();
        }
    }

    public async Task LoadAsync()
    {
        await repository.LoadAsync();
        Refresh();
    }

    private void Refresh()
    {
        var games = FilterIndex == 0
            ? repository.Games
            : repository.Games.Where(game => (int)game.Status == FilterIndex - 1);

        Games.Clear();
        foreach (var game in ((GameSortMode)SortIndex).Apply(games))
            Games.Add(game);
    }
}
