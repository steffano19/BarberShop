using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Models;

public class User
{
    public int ID { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;
}
