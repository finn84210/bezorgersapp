using bezorgersapp.ViewModels;

namespace bezorgersapp.Views;

public partial class CustomerPortalPage : ContentPage
{
    private readonly CustomerPortalViewModel _viewModel;

    public CustomerPortalPage(CustomerPortalViewModel viewModel)
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
