using bezorgersapp.ViewModels;

namespace bezorgersapp.Views;

public partial class EarningsPage : ContentPage
{
    private readonly EarningsViewModel _viewModel;

    public EarningsPage(EarningsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEarningsAsync();
    }

    private async void BackTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
