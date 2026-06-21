namespace bezorgersapp.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PackageSize { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string ExternalReference { get; set; } = string.Empty;
    public string DeliveryPerson { get; set; } = string.Empty;
    public DateTime? SentToDeliveryAt { get; set; }
    public DateTime ExpectedDeliveryTime { get; set; }
    public string Notes { get; set; } = string.Empty;
}
