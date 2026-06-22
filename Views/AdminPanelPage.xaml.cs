using bezorgersapp.ViewModels;

namespace bezorgersapp.Views;

public partial class AdminPanelPage : ContentPage
{
    private readonly AdminPanelViewModel _viewModel;

    public AdminPanelPage(AdminPanelViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Load();
    }
}
