using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Models;

public class BarberService
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Укажите название услуги")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Название от 5 до 100 символов")]
    public string Name { get; set; } = string.Empty;
    [Range(1, 100000, ErrorMessage = "Цена должна быть от 1 до 100 000")]
    public decimal Price { get; set; }
    [Range(5, 480, ErrorMessage = "Длительность от 5 до 480 минут")]
    public int DurationMinutes { get; set; }

}
