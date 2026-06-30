namespace bezorgersapp.Models;

public class CustomerOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ProductSummary { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Nieuw";
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = "Amsterdam";
    public string Notes { get; set; } = string.Empty;
    public bool IsSentToDeliveryApp { get; set; }
    public string AssignedVanName { get; set; } = string.Empty;
    public string AssignedVanLicensePlate { get; set; } = string.Empty;
    public string AssignedVanLoadingZone { get; set; } = string.Empty;
    public string AssignedVanFuelLevel { get; set; } = string.Empty;
    public List<DeliveryPackage> Packages { get; set; } = [];
}
