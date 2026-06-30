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
            new Customer { Id = 1, Name = "Sophie Janssen", Email = "sophie.janssen@example.nl" },
            new Customer { Id = 2, Name = "Youssef El Amrani", Email = "youssef.elamrani@example.nl" }
        ];

        Orders =
        [
            new CustomerOrder
            {
                Id = 1,
                CustomerId = 1,
                CustomerName = "Sophie Janssen",
                ProductSummary = "Trainingsshirt, Blauwe sporttas",
                TotalPrice = 44.45m,
                Status = "Demo: klaar voor bezorgapp",
                Address = "Stationsstraat 18",
                PostalCode = "6221 BP",
                City = "Maastricht",
                IsSentToDeliveryApp = true,
                AssignedVanName = "Mercedes Sprinter L2H2",
                AssignedVanLicensePlate = "V-842-FN",
                AssignedVanLoadingZone = "Laadpoort 2",
                AssignedVanFuelLevel = "82% diesel",
                Notes = "Appartement 3B. Aanbellen bij Janssen, pakket niet bij buren afgeven.",
                Packages =
                [
                    new DeliveryPackage { Id = 1, Description = "Doos 1/2 - Trainingsshirt maat M" },
                    new DeliveryPackage { Id = 2, Description = "Doos 2/2 - Blauwe sporttas" }
                ]
            },
            new CustomerOrder
            {
                Id = 2,
                CustomerId = 2,
                CustomerName = "Youssef El Amrani",
                ProductSummary = "Wedstrijdbal, Trainingsshirt",
                TotalPrice = 51.50m,
                Status = "Demo: klaar voor bezorgapp",
                Address = "Tongerseweg 132",
                PostalCode = "6214 BD",
                City = "Maastricht",
                IsSentToDeliveryApp = true,
                AssignedVanName = "Mercedes Sprinter L2H2",
                AssignedVanLicensePlate = "V-842-FN",
                AssignedVanLoadingZone = "Laadpoort 2",
                AssignedVanFuelLevel = "82% diesel",
                Notes = "Klant is na 15:00 thuis. Bij geen gehoor bellen op het nummer in de order.",
                Packages =
                [
                    new DeliveryPackage { Id = 1, Description = "Doos 1/2 - Wedstrijdbal maat 5" },
                    new DeliveryPackage { Id = 2, Description = "Doos 2/2 - Trainingsshirt maat L" }
                ]
            },
            new CustomerOrder
            {
                Id = 3,
                CustomerId = 1,
                CustomerName = "Emma Peeters",
                ProductSummary = "Teamset sokken, Keeperhandschoenen",
                TotalPrice = 38.90m,
                Status = "Demo: klaar voor bezorgapp",
                Address = "Markt 27",
                PostalCode = "6211 CK",
                City = "Maastricht",
                IsSentToDeliveryApp = true,
                AssignedVanName = "Volkswagen Transporter",
                AssignedVanLicensePlate = "V-317-LP",
                AssignedVanLoadingZone = "Laadpoort 1",
                AssignedVanFuelLevel = "64% diesel",
                Notes = "Afgeven bij receptie van het kantoor. Laat naam en tijd noteren.",
                Packages =
                [
                    new DeliveryPackage { Id = 1, Description = "Tas 1/2 - Teamset sokken" },
                    new DeliveryPackage { Id = 2, Description = "Doos 2/2 - Keeperhandschoenen" }
                ]
            }
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
            Status = "Nieuw",
            Address = "Hovenstraat 112",
            PostalCode = "1042 GE",
            Notes = "Klant is alleen na 15:00 thuis.",
            Packages = products.Select((product, index) => new DeliveryPackage
            {
                Id = index + 1,
                Description = product.Name
            }).ToList()
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

    public bool SendOrderToDeliveryApp(int id)
    {
        var order = Orders.FirstOrDefault(order => order.Id == id);
        if (order is null)
        {
            return false;
        }

        order.IsSentToDeliveryApp = true;
        order.Status = "Doorgestuurd naar bezorgapp";
        order.AssignedVanName = "Mercedes Sprinter 314";
        order.AssignedVanLicensePlate = "V-842-FN";
        order.AssignedVanLoadingZone = "Laadpoort B";
        order.AssignedVanFuelLevel = "82% diesel";
        return true;
    }

    public bool SavePackageCheck(int orderId, int packageId, bool isChecked)
    {
        var package = Orders
            .FirstOrDefault(order => order.Id == orderId)?
            .Packages
            .FirstOrDefault(package => package.Id == packageId);

        if (package is null)
        {
            return false;
        }

        package.IsChecked = isChecked;
        return true;
    }
}
