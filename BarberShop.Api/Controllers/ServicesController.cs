using BarberShop.Api.Data;
using BarberShop.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Api.Controllers;


[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ServicesController(AppDbContext db)
    {
        _db = db;
    }

[HttpGet]
    public async Task<ActionResult<List<BarberService>>> GetAll()
    {
        var services = await _db.Services.ToListAsync();
        return Ok(services);
    }

}
