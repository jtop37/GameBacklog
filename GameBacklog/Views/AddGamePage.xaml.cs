using GameBacklog.ViewModels;

namespace GameBacklog.Views;

public partial class AddGamePage : ContentPage
{
    public AddGamePage(AddGameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
