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
}
