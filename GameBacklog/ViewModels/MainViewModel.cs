using System.Collections.ObjectModel;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly GameRepository repository;

    public MainViewModel(GameRepository repository)
    {
        this.repository = repository;
    }

    public ObservableCollection<Game> Games => repository.Games;

    public Task LoadAsync() => repository.LoadAsync();
}
