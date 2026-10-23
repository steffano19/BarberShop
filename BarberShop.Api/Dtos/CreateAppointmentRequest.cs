using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Dtos;

public class CreateAppointmentRequest
{
    [Range(1, int.MaxValue, ErrorMessage ="Укажите мастера")]
    public int BarberId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Укажите услугу")]
    public int ServiceId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя от 2 до 100 символов")]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Телефон: от 10 до 15 цифр, можно с + в начале")]
    public string ClientPhone { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
}
