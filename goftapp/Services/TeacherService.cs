using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Infrastructure;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public class TeacherService : ITeacherService
{
    private readonly AppDbContext _Db;

    public TeacherService(AppDbContext db)
    {
        _Db = db;
    }
    
    public async Task<(Users user, Teacher teacher)> CreateTeacherAsync(CreateTeacherUserDto dto, Guid createdByAdminUserId, CancellationToken ct = default)
    {
        // check Username
        if (await _Db.Users.AnyAsync(x => x.Username == dto.Username, ct))
            throw new InvalidOperationException("Username Exists.");
        var user = new Users
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Teacher,
            IsActive = true
        };

        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Department = dto.Department,
            CreatedByAdminUserId = createdByAdminUserId,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        user.TeacherId = teacher.Id;
        _Db.Users.Add(user);
        _Db.Teachers.Add(teacher);
        await _Db.SaveChangesAsync(ct);
        return (user, teacher);
    }
}
