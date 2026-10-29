using GameBacklog.Models;
using GameBacklog.ViewModels;

namespace GameBacklog.Views;

[QueryProperty(nameof(GameId), "id")]
public partial class GameDetailPage : ContentPage
{
    private readonly GameDetailViewModel viewModel;

    public GameDetailPage(GameDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    public string GameId
    {
        set
        {
            if (Guid.TryParse(value, out var id))
                _ = viewModel.LoadAsync(id);
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e) =>
        await Shell.Current.GoToAsync("..");

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        var confirmed = await DisplayAlertAsync("Delete game", "Remove this game from your library?", "Delete", "Cancel");
        if (!confirmed)
            return;

        await viewModel.DeleteAsync();
        await Shell.Current.GoToAsync("..");
    }

    private async void OnLogPlaytimeClicked(object? sender, EventArgs e) =>
        await viewModel.LogPlaytimeAsync();

    private async void OnAddAchievementClicked(object? sender, EventArgs e) =>
        await viewModel.AddAchievementAsync();

    private async void OnToggleAchievementClicked(object? sender, EventArgs e)
    {
        if (sender is Button { BindingContext: Achievement achievement })
            await viewModel.ToggleAchievementAsync(achievement);
    }
}
