using System.ComponentModel.DataAnnotations;

namespace BarberShop.Api.Models;

public class Barber
{
    public int ID { get; set;}

    [Required(ErrorMessage = "Укажите имя мастера")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя от 2 до 100 символов")]
    public string Name { get; set; } = string.Empty;

    public List<BarberService> Services { get; set; } = new();
}
