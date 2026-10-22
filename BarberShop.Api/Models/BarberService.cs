namespace BarberShop.Api.Models;

public class BarberService
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }

}
