using System.Collections.ObjectModel;
using System.Windows.Input;
using bezorgersapp.Models;
using bezorgersapp.Services;

namespace bezorgersapp.ViewModels;

public class CustomerPortalViewModel : BaseViewModel
{
    private readonly AuthService _authService;
    private readonly StoreService _storeService;
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _message = string.Empty;

    public CustomerPortalViewModel(AuthService authService, StoreService storeService)
    {
        _authService = authService;
        _storeService = storeService;
        Products = [];
        Cart = [];
        OrderHistory = [];
        RegisterCommand = new Command(Register);
        AddToCartCommand = new Command<Product>(AddToCart);
        RemoveFromCartCommand = new Command<Product>(RemoveFromCart);
        PlaceOrderCommand = new Command(PlaceOrder);
        LoadCommand = new Command(Load);
    }

    public ObservableCollection<Product> Products { get; }
    public ObservableCollection<Product> Cart { get; }
    public ObservableCollection<CustomerOrder> OrderHistory { get; }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public decimal TotalPrice => Cart.Sum(product => product.Price);

    public ICommand RegisterCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand RemoveFromCartCommand { get; }
    public ICommand PlaceOrderCommand { get; }
    public ICommand LoadCommand { get; }

    public void Load()
    {
        Products.Clear();
        foreach (var product in _storeService.Products)
        {
            Products.Add(product);
        }

        RefreshOrders();
    }

    private void Register()
    {
        Message = _authService.RegisterCustomer(Name, Email, Password, out var message)
            ? message
            : message;
        RefreshOrders();
    }

    private void AddToCart(Product? product)
    {
        if (product is null)
        {
            return;
        }

        Cart.Add(product);
        OnPropertyChanged(nameof(TotalPrice));
        Message = $"{product.Name} toegevoegd aan winkelwagen.";
    }

    private void RemoveFromCart(Product? product)
    {
        if (product is not null && Cart.Remove(product))
        {
            OnPropertyChanged(nameof(TotalPrice));
            Message = $"{product.Name} verwijderd uit winkelwagen.";
        }
    }

    private void PlaceOrder()
    {
        if (Cart.Count == 0)
        {
            Message = "Voeg eerst producten toe aan de winkelwagen.";
            return;
        }

        var customer = _storeService.Customers.FirstOrDefault(customer =>
            customer.Email.Equals(_authService.CurrentUser?.Email ?? "klant@matrix.nl", StringComparison.OrdinalIgnoreCase)) ??
            _storeService.Customers.First();

        _storeService.PlaceOrder(customer, Cart.ToList());
        Cart.Clear();
        OnPropertyChanged(nameof(TotalPrice));
        RefreshOrders();
        Message = "Bestelling geplaatst.";
    }

    private void RefreshOrders()
    {
        OrderHistory.Clear();
        var email = _authService.CurrentUser?.Email ?? "klant@matrix.nl";
        var customer = _storeService.Customers.FirstOrDefault(customer =>
            customer.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        var orders = customer is null
            ? _storeService.Orders
            : _storeService.Orders.Where(order => order.CustomerId == customer.Id);

        foreach (var order in orders)
        {
            OrderHistory.Add(order);
        }
    }
}
