using System.Collections.ObjectModel;
using System.Windows.Input;
using bezorgersapp.Models;
using bezorgersapp.Services;
using bezorgersapp.Views;

namespace bezorgersapp.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    private readonly OrderService _orderService;
    private decimal _totalEarned;
    private int _deliveryCount;

    public DashboardViewModel(OrderService orderService)
    {
        _orderService = orderService;
        RecentOrders = [];
        LoadDashboardCommand = new Command(async () => await LoadDashboardAsync());
        GoToTasksCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}"));
        GoToEarningsCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(EarningsPage)));
    }

    public ObservableCollection<Order> RecentOrders { get; }

    public decimal TotalEarned
    {
        get => _totalEarned;
        set => SetProperty(ref _totalEarned, value);
    }

    public int DeliveryCount
    {
        get => _deliveryCount;
        set => SetProperty(ref _deliveryCount, value);
    }

    public ICommand LoadDashboardCommand { get; }
    public ICommand GoToTasksCommand { get; }
    public ICommand GoToEarningsCommand { get; }

    public async Task LoadDashboardAsync()
    {
        var orders = await _orderService.GetOrdersAsync();
        DeliveryCount = orders.Count;
        TotalEarned = orders.Sum(order => order.Fee);

        RecentOrders.Clear();
        foreach (var order in orders.Take(2))
        {
            RecentOrders.Add(order);
        }
    }
}

