using BarberShop.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Api.Controllers;


[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private static readonly List<BarberService> Services =
    [
        new() { ID = 1, Name = "Мужская стрижка", Price = 1200, DurationMinutes = 45 },
        new() { ID = 2, Name = "Стрижка бороды", Price = 700, DurationMinutes = 30 },
        new() { ID = 3, Name = "Стрижка + борода", Price = 1700, DurationMinutes = 75 }
    ];

    [HttpGet]
    public ActionResult<List<BarberService>> GetAll()
    {
        return Ok(Services);
    }

}
