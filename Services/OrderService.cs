using bezorgersapp.Models;

namespace bezorgersapp.Services;

public class OrderService
{
    private readonly StoreService _storeService;

    public OrderService(StoreService storeService)
    {
        _storeService = storeService;
    }

    public Task<List<Order>> GetOrdersAsync()
    {
        var orders = _storeService.Orders
            .Where(order => order.IsSentToDeliveryApp)
            .Select(ToDeliveryOrder)
            .ToList();

        foreach (var order in orders)
        {
            ApplySavedState(order);
        }

        return Task.FromResult(orders);
    }

    public Task<Order?> GetOrderByIdAsync(int id)
    {
        var order = _storeService.Orders
            .Where(order => order.IsSentToDeliveryApp)
            .Select(ToDeliveryOrder)
            .FirstOrDefault(order => order.Id == id);

        if (order is not null)
        {
            ApplySavedState(order);
        }

        return Task.FromResult(order);
    }

    public Task<bool> UpdateOrderStatusAsync(int id, string status)
    {
        var updateGelukt = _storeService.UpdateOrderStatus(id, status);

        if (!updateGelukt)
        {
            return Task.FromResult(false);
        }

        Preferences.Set(StatusKey(id), status);
        return Task.FromResult(true);
    }

    public Task SaveLocationAsync(int id, string locationText)
    {
        Preferences.Set(LocationKey(id), locationText);
        return Task.CompletedTask;
    }

    public Task SaveDeliveryPhotoAsync(int id, string photoPath)
    {
        Preferences.Set(PhotoKey(id), photoPath);
        return Task.CompletedTask;
    }

    public Task SavePackageCheckAsync(int orderId, int packageId, bool isChecked)
    {
        _storeService.SavePackageCheck(orderId, packageId, isChecked);
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

    private static Order ToDeliveryOrder(CustomerOrder order)
    {
        return new Order
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Address = order.Address,
            PostalCode = order.PostalCode,
            City = order.City,
            PackageSize = $"{order.Packages.Count} pakket(ten)",
            Fee = 4.95m + order.Packages.Count,
            Status = order.Status,
            Source = "Admin doorgestuurd",
            ExternalReference = $"ADMIN-{order.Id:0000}",
            DeliveryPerson = string.Empty,
            SentToDeliveryAt = DateTime.Now,
            ExpectedDeliveryTime = DateTime.Today.AddHours(16),
            Notes = order.Notes,
            VanName = order.AssignedVanName,
            VanLicensePlate = order.AssignedVanLicensePlate,
            VanLoadingZone = order.AssignedVanLoadingZone,
            VanFuelLevel = order.AssignedVanFuelLevel,
            Packages = order.Packages
        };
    }
}
