using GameBacklog.Models;
using GameBacklog.ViewModels;

namespace GameBacklog.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadAsync();
    }

    private async void OnAddClicked(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync("add");

    private async void OnGameSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Game game)
            return;

        GamesList.SelectedItem = null;
        await Shell.Current.GoToAsync($"game?id={game.Id}");
    }
}
