using bezorgersapp.Models;

namespace bezorgersapp.Services;

public class OrderService
{
    // Voor deze opdracht gebruik ik lokale voorbeelddata.
    // Later kan deze lijst vervangen worden door data uit een backend.
    private readonly List<Order> _orders =
    [
        new Order
        {
            Id = 1,
            CustomerName = "Neo Anderson",
            Address = "Europalaan 45",
            PostalCode = "1056 CP",
            City = "Amsterdam",
            PackageSize = "Klein pakket",
            Fee = 4.15m,
            Status = "Doorgegeven aan bezorger",
            Source = "Webshop",
            ExternalReference = "SE1-WEB-1001",
            DeliveryPerson = "Bezorger",
            SentToDeliveryAt = DateTime.Today.AddHours(8).AddMinutes(15),
            ExpectedDeliveryTime = DateTime.Today.AddHours(14).AddMinutes(30),
            Notes = "Bel aan bij de hoofdingang."
        },
        new Order
        {
            Id = 2,
            CustomerName = "Trinity Moss",
            Address = "Bezemweg 12",
            PostalCode = "1051 AP",
            City = "Amsterdam",
            PackageSize = "Middel pakket",
            Fee = 6.45m,
            Status = "Onderweg",
            Source = "Back-office",
            ExternalReference = "SE2-ADMIN-2044",
            DeliveryPerson = "Bezorger",
            SentToDeliveryAt = DateTime.Today.AddHours(8).AddMinutes(40),
            ExpectedDeliveryTime = DateTime.Today.AddHours(15).AddMinutes(15),
            Notes = "Pakket mag in de tuin worden gelegd."
        },
        new Order
        {
            Id = 3,
            CustomerName = "Morpheus Smith",
            Address = "Hovenstraat 112",
            PostalCode = "1042 GE",
            City = "Amsterdam",
            PackageSize = "Groot pakket",
            Fee = 5.95m,
            Status = "Niet bezorgd",
            Source = "Webshop",
            ExternalReference = "SE1-WEB-1003",
            DeliveryPerson = "Bezorger",
            SentToDeliveryAt = DateTime.Today.AddHours(9),
            ExpectedDeliveryTime = DateTime.Today.AddHours(16),
            Notes = "Klant is alleen na 15:00 thuis."
        }
    ];

    public Task<List<Order>> GetOrdersAsync()
    {
        foreach (var order in _orders)
        {
            ApplySavedState(order);
        }

        return Task.FromResult(_orders);
    }

    public Task<Order?> GetOrderByIdAsync(int id)
    {
        var order = _orders.FirstOrDefault(order => order.Id == id);
        if (order is not null)
        {
            ApplySavedState(order);
        }

        return Task.FromResult(order);
    }

    public Task<bool> UpdateOrderStatusAsync(int id, string status)
    {
        var order = _orders.FirstOrDefault(order => order.Id == id);

        if (order is null)
        {
            return Task.FromResult(false);
        }

        order.Status = status;
        Preferences.Set(StatusKey(id), status);
        return Task.FromResult(true);
    }

    public Task SaveLocationAsync(int id, string locationText)
    {
        var order = _orders.FirstOrDefault(order => order.Id == id);
        if (order is not null)
        {
            order.LocationText = locationText;
            Preferences.Set(LocationKey(id), locationText);
        }

        return Task.CompletedTask;
    }

    public Task SaveDeliveryPhotoAsync(int id, string photoPath)
    {
        var order = _orders.FirstOrDefault(order => order.Id == id);
        if (order is not null)
        {
            order.DeliveryPhotoPath = photoPath;
            Preferences.Set(PhotoKey(id), photoPath);
        }

        return Task.CompletedTask;
    }

    private static void ApplySavedState(Order order)
    {
        order.Status = Preferences.Get(StatusKey(order.Id), order.Status);
        order.LocationText = Preferences.Get(LocationKey(order.Id), order.LocationText);
        order.DeliveryPhotoPath = Preferences.Get(PhotoKey(order.Id), order.DeliveryPhotoPath);
    }

    private static string StatusKey(int id) => $"order_{id}_status";
    private static string LocationKey(int id) => $"order_{id}_location";
    private static string PhotoKey(int id) => $"order_{id}_photo";
}
