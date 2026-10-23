namespace BarberShop.Api.Common;

public static class WorkingHours
{
    public static readonly TimeSpan Open = new(10, 0, 0);
    public static readonly TimeSpan Close = new(20, 0, 0);
    public static readonly TimeSpan SlotStep = TimeSpan.FromMinutes(15);
}
