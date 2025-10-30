using goftapp.Entity;
using goftapp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        //สร้างฐานถ้ายังไม่มี
        await db.Database.MigrateAsync();
        
        // ตรวจว่ามี admin แล้วหรือยัง
        if (await db.Users.AnyAsync(u => u.Role == UserRole.Admin))
        {
            return; // มีแล้ว ไม่ต้องทำซ้ำ
        }
        
        // สร้าง user admin
        var admin = new Users
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin1234"),
            Role = UserRole.Admin,
            IsActive = true
        };
        
        db.Users.Add(admin);
        await db.SaveChangesAsync();
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Seeded default admin: username='admin' password='admin1234'");
        Console.ResetColor();
    }
}