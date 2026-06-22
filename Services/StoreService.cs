using bezorgersapp.Models;

namespace bezorgersapp.Services;

public class StoreService
{
    private int _nextProductId = 4;
    private int _nextCustomerId = 3;
    private int _nextOrderId = 3;

    public StoreService()
    {
        Products =
        [
            new Product { Id = 1, Name = "Blauwe sporttas", Price = 24.95m, Stock = 8 },
            new Product { Id = 2, Name = "Trainingsshirt", Price = 19.50m, Stock = 12 },
            new Product { Id = 3, Name = "Wedstrijdbal", Price = 32.00m, Stock = 5 }
        ];

        Customers =
        [
            new Customer { Id = 1, Name = "Klant", Email = "klant@matrix.nl" },
            new Customer { Id = 2, Name = "Neo Anderson", Email = "neo@example.nl" }
        ];

        Orders =
        [
            new CustomerOrder { Id = 1, CustomerId = 1, CustomerName = "Klant", ProductSummary = "Trainingsshirt", TotalPrice = 19.50m, Status = "Nieuw" },
            new CustomerOrder { Id = 2, CustomerId = 2, CustomerName = "Neo Anderson", ProductSummary = "Blauwe sporttas, Wedstrijdbal", TotalPrice = 56.95m, Status = "In behandeling" }
        ];
    }

    public List<Product> Products { get; }
    public List<Customer> Customers { get; }
    public List<CustomerOrder> Orders { get; }

    public Customer AddCustomer(string name, string email)
    {
        var customer = new Customer { Id = _nextCustomerId++, Name = name, Email = email };
        Customers.Add(customer);
        return customer;
    }

    public Product AddProduct(string name, decimal price, int stock)
    {
        var product = new Product { Id = _nextProductId++, Name = name, Price = price, Stock = stock };
        Products.Add(product);
        return product;
    }

    public bool UpdateProduct(int id, string name, decimal price, int stock)
    {
        var product = Products.FirstOrDefault(product => product.Id == id);
        if (product is null)
        {
            return false;
        }

        product.Name = name;
        product.Price = price;
        product.Stock = stock;
        return true;
    }

    public bool RemoveProduct(int id)
    {
        var product = Products.FirstOrDefault(product => product.Id == id);
        return product is not null && Products.Remove(product);
    }

    public CustomerOrder PlaceOrder(Customer customer, IReadOnlyCollection<Product> products)
    {
        var order = new CustomerOrder
        {
            Id = _nextOrderId++,
            CustomerId = customer.Id,
            CustomerName = customer.Name,
            ProductSummary = string.Join(", ", products.Select(product => product.Name)),
            TotalPrice = products.Sum(product => product.Price),
            Status = "Nieuw"
        };

        Orders.Add(order);
        return order;
    }

    public bool UpdateOrderStatus(int id, string status)
    {
        var order = Orders.FirstOrDefault(order => order.Id == id);
        if (order is null)
        {
            return false;
        }

        order.Status = status;
        return true;
    }
}
