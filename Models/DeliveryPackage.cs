namespace bezorgersapp.Models;

public class DeliveryPackage
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsChecked { get; set; }
}
