using System.Collections.ObjectModel;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private int sortIndex;

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

    public async Task LoadAsync()
    {
        await repository.LoadAsync();
        Refresh();
    }

    private void Refresh()
    {
        Games.Clear();
        foreach (var game in ((GameSortMode)SortIndex).Apply(repository.Games))
            Games.Add(game);
    }
}
