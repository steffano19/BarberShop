using BarberShop.Api.Common;
using BarberShop.Api.Data;
using BarberShop.Api.Dtos;
using BarberShop.Api.Models;
using BarberShop.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Api.Controllers;




[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TokenService _tokens;



    public AuthController(AppDbContext db, TokenService tokens)
    {
        _db = db;
        _tokens = tokens;

    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        bool exists = await _db.Users.AnyAsync(u => u.Phone == request.Phone);
        if (exists)
            return Conflict("Пользователь с таким телефоном уже зарегистрирован");

        var user = new User
        {
            Name = request.Name,
            Phone = request.Phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = Roles.Client
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return new AuthResponse { Token = _tokens.CreateToken(user), Name = user.Name, Role = user.Role };
    }


    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Неверный телефон или пароль");

        return new AuthResponse { Token = _tokens.CreateToken(user), Name = user.Name, Role = user.Role };
    }

}

