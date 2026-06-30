using System.Collections.ObjectModel;
using System.Windows.Input;
using bezorgersapp.Models;
using bezorgersapp.Services;
using bezorgersapp.Views;

namespace bezorgersapp.ViewModels;

public class OrdersViewModel : BaseViewModel
{
    private readonly OrderService _orderService;
    private string _message = string.Empty;

    public OrdersViewModel(OrderService orderService)
    {
        _orderService = orderService;
        Orders = [];
        LoadOrdersCommand = new Command(async () => await LoadOrdersAsync());
        OpenOrderCommand = new Command<Order>(async order => await OpenOrderAsync(order));
    }

    public ObservableCollection<Order> Orders { get; }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ICommand LoadOrdersCommand { get; }
    public ICommand OpenOrderCommand { get; }

    public async Task LoadOrdersAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Message = "Bestellingen laden...";

            var orders = await _orderService.GetOrdersAsync();

            Orders.Clear();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }

            Message = Orders.Count == 0
                ? "Er zijn nog geen demo-bestellingen voor de bezorgapp."
                : string.Empty;
        }
        catch
        {
            Message = "Bestellingen konden niet worden geladen. Probeer het later opnieuw.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task OpenOrderAsync(Order? order)
    {
        if (order is null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?id={order.Id}");
    }
}
