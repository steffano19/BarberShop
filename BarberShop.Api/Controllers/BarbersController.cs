using BarberShop.Api.Common;
using BarberShop.Api.Data;
using BarberShop.Api.Dtos;
using BarberShop.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Api.Controllers;



[ApiController]
[Route("api/barbers")]
public class BarbersController : ControllerBase
{
    private readonly AppDbContext _db;

    public BarbersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Barber>>> GetAll()
    {
        var barbers = await _db.Barbers
            .Include(b => b.Services)
            .ToListAsync();
        return Ok(barbers);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<Barber>> GetById(int id)
    {
        var barber = await _db.Barbers
            .Include(b => b.Services)
            .FirstOrDefaultAsync(b => b.ID == id);
        if (barber == null)
            return NotFound();

        return Ok(barber);
    }

    [HttpPost]
    public async Task<ActionResult<Barber>> Create(CreateBarberRequest request)
    {
        var services = await _db.Services
            .Where(s => request.ServiceIds.Contains(s.ID))
            .ToListAsync();

        if (services.Count != request.ServiceIds.Distinct().Count())
            return BadRequest("Одна или несколько услуг не найдены");

        var barber = new Barber { Name = request.Name, Services = services };
        _db.Barbers.Add(barber);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = barber.ID }, barber);

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var barber = await _db.Barbers.FindAsync(id);
        if (barber == null)
            return NotFound();

        _db.Barbers.Remove(barber);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id:int}/slots")]
    public async Task<ActionResult<List<string>>> GetFreeSlots(
        int id, [FromQuery] DateOnly date, [FromQuery] int serviceId)
    {
        var barber = await _db.Barbers
            .Include(b => b.Services)
            .FirstOrDefaultAsync(b => b.ID == id);
        if (barber == null)
            return NotFound("Мастер не найден");
        var service = barber.Services.FirstOrDefault(s => s.ID == serviceId);
        if (service == null)
            return BadRequest("Этот мастер не оказывает такую услугу");
        var dayStart = date.ToDateTime(TimeOnly.MinValue);
        var dayEnd = dayStart.AddDays(1);
        var appointments = await _db.Appointments
            .Where(a => a.BarberID == id && a.StartTime < dayEnd && a.EndTime > dayStart)
            .ToListAsync();
        var duration = TimeSpan.FromMinutes(service.DurationMinutes);
        var slotStart = dayStart + WorkingHours.Open;
        var closing = dayStart + WorkingHours.Close;
        var freeSlots = new List<string>();
        while (slotStart + duration <= closing)
        {
            var slotEnd = slotStart + duration;
            bool isPast = slotStart <= DateTime.Now;
            bool isBusy = appointments.Any(a => a.StartTime < slotEnd && slotStart < a.EndTime);
            if (!isPast && !isBusy)
                freeSlots.Add(slotStart.ToString("HH:mm"));
            slotStart += WorkingHours.SlotStep;
        }
        return freeSlots;
    }

}
