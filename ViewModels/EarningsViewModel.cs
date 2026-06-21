using bezorgersapp.Services;

namespace bezorgersapp.ViewModels;

public class EarningsViewModel : BaseViewModel
{
    private readonly OrderService _orderService;
    private decimal _totalEarned;
    private decimal _deliveryEarned;
    private decimal _bonusEarned;
    private decimal _tipsEarned;

    public EarningsViewModel(OrderService orderService)
    {
        _orderService = orderService;
    }

    public decimal TotalEarned
    {
        get => _totalEarned;
        set => SetProperty(ref _totalEarned, value);
    }

    public decimal DeliveryEarned
    {
        get => _deliveryEarned;
        set => SetProperty(ref _deliveryEarned, value);
    }

    public decimal BonusEarned
    {
        get => _bonusEarned;
        set => SetProperty(ref _bonusEarned, value);
    }

    public decimal TipsEarned
    {
        get => _tipsEarned;
        set => SetProperty(ref _tipsEarned, value);
    }

    public async Task LoadEarningsAsync()
    {
        var orders = await _orderService.GetOrdersAsync();

        DeliveryEarned = orders.Sum(order => order.Fee);
        BonusEarned = 10.50m;
        TipsEarned = 4.95m;
        TotalEarned = DeliveryEarned + BonusEarned + TipsEarned;
    }
}
