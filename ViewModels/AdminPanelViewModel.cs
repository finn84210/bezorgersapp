using System.Collections.ObjectModel;
using System.Windows.Input;
using bezorgersapp.Models;
using bezorgersapp.Services;

namespace bezorgersapp.ViewModels;

public class AdminPanelViewModel : BaseViewModel
{
    private readonly StoreService _storeService;
    private string _productName = string.Empty;
    private string _productPrice = string.Empty;
    private string _productStock = string.Empty;
    private string _message = string.Empty;
    private Product? _selectedProduct;

    public AdminPanelViewModel(StoreService storeService)
    {
        _storeService = storeService;
        Products = [];
        Customers = [];
        Orders = [];
        LoadCommand = new Command(Load);
        AddProductCommand = new Command(AddProduct);
        ShowProductDetailsCommand = new Command<Product>(ShowProductDetails);
        RemoveProductCommand = new Command<Product>(RemoveProduct);
        ProcessOrderCommand = new Command<CustomerOrder>(order => UpdateOrderStatus(order, "In behandeling"));
        SendOrderCommand = new Command<CustomerOrder>(PickOrderForDelivery);
    }

    public ObservableCollection<Product> Products { get; }
    public ObservableCollection<Customer> Customers { get; }
    public ObservableCollection<CustomerOrder> Orders { get; }

    public string ProductName
    {
        get => _productName;
        set => SetProperty(ref _productName, value);
    }

    public string ProductPrice
    {
        get => _productPrice;
        set => SetProperty(ref _productPrice, value);
    }

    public string ProductStock
    {
        get => _productStock;
        set => SetProperty(ref _productStock, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public Product? SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (SetProperty(ref _selectedProduct, value))
            {
                OnPropertyChanged(nameof(HasSelectedProduct));
            }
        }
    }

    public bool HasSelectedProduct => SelectedProduct is not null;

    public ICommand LoadCommand { get; }
    public ICommand AddProductCommand { get; }
    public ICommand ShowProductDetailsCommand { get; }
    public ICommand RemoveProductCommand { get; }
    public ICommand ProcessOrderCommand { get; }
    public ICommand SendOrderCommand { get; }

    public void Load()
    {
        Refresh();
    }

    private void AddProduct()
    {
        if (string.IsNullOrWhiteSpace(ProductName) ||
            !decimal.TryParse(ProductPrice.Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var price) ||
            !int.TryParse(ProductStock, out var stock))
        {
            Message = "Vul een productnaam, prijs en voorraad in.";
            return;
        }

        _storeService.AddProduct(ProductName.Trim(), price, stock);
        ProductName = string.Empty;
        ProductPrice = string.Empty;
        ProductStock = string.Empty;
        Message = "Product toegevoegd.";
        Refresh();
    }

    private void ShowProductDetails(Product? product)
    {
        SelectedProduct = product;
        if (product is not null)
        {
            Message = $"Details geopend voor {product.Name}.";
        }
    }

    private void RemoveProduct(Product? product)
    {
        if (product is not null && _storeService.RemoveProduct(product.Id))
        {
            SelectedProduct = null;
            Message = "Product verwijderd.";
            Refresh();
        }
    }

    private void UpdateOrderStatus(CustomerOrder? order, string status)
    {
        if (order is not null && _storeService.UpdateOrderStatus(order.Id, status))
        {
            Message = "Bestelstatus aangepast.";
            Refresh();
        }
    }

    private void PickOrderForDelivery(CustomerOrder? order)
    {
        if (order is not null && _storeService.PickOrderForDelivery(order.Id))
        {
            Message = "Bestelling gepickt en zichtbaar gezet voor de bezorger.";
            Refresh();
        }
    }

    private void Refresh()
    {
        Products.Clear();
        Customers.Clear();
        Orders.Clear();

        foreach (var product in _storeService.Products)
        {
            Products.Add(product);
        }

        foreach (var customer in _storeService.Customers)
        {
            Customers.Add(customer);
        }

        foreach (var order in _storeService.Orders)
        {
            Orders.Add(order);
        }
    }
}
