using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Dtos;

public class LoginRequest
{
    [Required]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

}
