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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BarberService>> GetById(int id)
    {
        var service = await _db.Services.FindAsync(id);
        if (service == null)
            return NotFound();

        return Ok(service);
    }

    [HttpPost]
    public async Task<ActionResult<BarberService>> Create(BarberService service)
    {
        _db.Services.Add(service);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = service.ID }, service);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update (int id, BarberService updated)
    {
        var service = await _db.Services.FindAsync(id);
        if (service == null)
            return NotFound();

        service.Name = updated.Name;
        service.Price = updated.Price;
        service.DurationMinutes = updated.DurationMinutes;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var service = await _db.Services.FindAsync(id);
        if (service == null)
            return NotFound();

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


