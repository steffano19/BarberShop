using BarberShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberShop.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<BarberService> Services => Set<BarberService>();
    public DbSet<Barber> Barbers => Set<Barber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BarberService>()
            .Property(s => s.Price)
            .HasPrecision(10, 2);


        modelBuilder.Entity<Barber>()
            .HasMany(b => b.Services)
            .WithMany();

        modelBuilder.Entity<BarberService>().HasData(
            new BarberService { ID = 1, Name = "Мужская стрижка", Price = 1200, DurationMinutes = 45 },
            new BarberService { ID = 2, Name = "Стрижка бороды", Price = 700, DurationMinutes = 30 },
            new BarberService { ID = 3, Name = "Стрижка + борода", Price = 1700, DurationMinutes = 75 }
        );
    }


}
