namespace bezorgersapp.Models;

public class CustomerOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ProductSummary { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Nieuw";
}
