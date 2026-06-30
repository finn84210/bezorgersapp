using bezorgersapp.ViewModels;
using bezorgersapp.Models;

namespace bezorgersapp.Views;

public partial class OrderDetailPage : ContentPage
{
    public OrderDetailPage(OrderDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void BackTapped(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void PackageCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox &&
            checkBox.BindingContext is DeliveryPackage package &&
            BindingContext is OrderDetailViewModel viewModel)
        {
            await viewModel.SavePackageCheckAsync(package, e.Value);
        }
    }
}
