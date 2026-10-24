using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Dtos;

public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя от 2 до 100 символов")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Телефон: от 10 до 15 цифр, можно с + в начале")]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль не короче 6 символов")]
    public string Password { get; set; } = string.Empty;
}
