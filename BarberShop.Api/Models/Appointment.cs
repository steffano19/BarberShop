namespace BarberShop.Api.Models;

public class Appointment
{
    public int ID { get; set; }

    public int BarberID { get; set; }
    public Barber Barber { get; set; } = null!;

    public int ServiceID { get; set; }
    public BarberService Service { get; set; } = null!;

    public string ClientName { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

}
