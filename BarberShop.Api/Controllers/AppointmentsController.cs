using BarberShop.Api.Data;
using BarberShop.Api.Dtos;
using BarberShop.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Api.Controllers;




[ApiController]
[Route("api/appointments")]
public class AppointmentsController : ControllerBase
{
    private static readonly TimeSpan OpenTime = new(10, 0, 0);
    private static readonly TimeSpan CloseTime = new(20, 0, 0);

    private readonly AppDbContext _db;

    public AppointmentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Appointment>>> GetAll([FromQuery] int? barberId)
    {
        var query = _db.Appointments
            .Include(a => a.Barber)
            .Include(a => a.Service)
            .AsQueryable();

        if (barberId != null)
            query = query.Where(a => a.BarberID == barberId);

        return await query.OrderBy(a => a.StartTime).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetById(int id)
    {
        var appointment = await _db.Appointments
            .Include(a => a.Barber)
            .Include(a => a.Service)
            .FirstOrDefaultAsync(a => a.ID == id);

        if (appointment == null)
            return NotFound();

        return appointment;
    }


    [HttpPost]
    public async Task<ActionResult<Appointment>> Create(CreateAppointmentRequest request)
    {
        if (request.StartTime <= DateTime.Now)
            return BadRequest("Нельзя записаться на прошедшее время");

        var barber = await _db.Barbers
            .Include(b => b.Services)
            .FirstOrDefaultAsync(b => b.ID == request.BarberId);
        if (barber == null)
            return BadRequest("Мастер не найден");


        var service = barber.Services.FirstOrDefault(s => s.ID == request.ServiceId);
        if (service == null)
            return BadRequest("Этот мастер не оказывает такую услугу");


        var start = request.StartTime;
        var end = start.AddMinutes(service.DurationMinutes);

        if (start.TimeOfDay < OpenTime || end.Date != start.Date || end.TimeOfDay > CloseTime)
            return BadRequest("Запись возможна только с 10:00 до 20:00");


        bool isBusy = await _db.Appointments.AnyAsync(a =>
            a.BarberID == barber.ID &&
            a.StartTime < end &&
            start < a.EndTime);
        if (isBusy)
            return Conflict("Мастер уже занят в это время");


        var appointment = new Appointment
        {
            BarberID = barber.ID,
            ServiceID = service.ID,
            ClientName = request.ClientName,
            ClientPhone = request.ClientPhone,
            StartTime = start,
            EndTime = end
        };

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = appointment.ID }, appointment);

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment == null)
            return NotFound();

        _db.Appointments.Remove(appointment);
        await _db.SaveChangesAsync();
        return NoContent();
    }


}
