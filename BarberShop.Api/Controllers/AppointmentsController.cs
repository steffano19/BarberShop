using BarberShop.Api.Common;
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
        // 1. Нельзя записаться в прошлое
        if (request.StartTime <= DateTime.Now)
            return BadRequest("Нельзя записаться на прошедшее время");

        // 2. Мастер существует?
        var barber = await _db.Barbers
            .Include(b => b.Services)
            .FirstOrDefaultAsync(b => b.ID == request.BarberId);
        if (barber == null)
            return BadRequest("Мастер не найден");

        // 3. Мастер оказывает эту услугу?
        var service = barber.Services.FirstOrDefault(s => s.ID == request.ServiceId);
        if (service == null)
            return BadRequest("Этот мастер не оказывает такую услугу");

        // 4. Конец записи = начало + длительность услуги
        var start = request.StartTime;

        // Время должно быть кратно 15 минутам
        if (start.Minute % 15 != 0 || start.Second != 0)
            return BadRequest("Время записи должно быть кратно 15 минутам (12:00, 12:15, 12:30...)");

        var end = start.AddMinutes(service.DurationMinutes);

        // 5. Укладываемся в рабочее время?
        if (start.TimeOfDay < WorkingHours.Open || end.Date != start.Date || end.TimeOfDay > WorkingHours.Close)
            return BadRequest("Запись возможна только с 10:00 до 20:00");

        // 6. Мастер свободен?
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
