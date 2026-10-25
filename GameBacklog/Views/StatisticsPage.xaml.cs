using GameBacklog.ViewModels;

namespace GameBacklog.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel viewModel;

    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.RefreshAsync();
    }
}
