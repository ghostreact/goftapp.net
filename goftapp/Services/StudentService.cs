using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Infrastructure;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _Db;

    public StudentService(AppDbContext db)
    {
        _Db = db;
    }
    
    public async Task<Student> CreateStudent(CreateStudentDto dto, CancellationToken ct = default)
    {
        if (await _Db.Students.AnyAsync(x => x.StudentCode == dto.StudentCode, ct))
            throw new InvalidOperationException("StudentCode Exists");
        
        // valid level class / years/ room
        if (dto.Room < 1 || dto.Room > 10)
        {
            throw new InvalidOperationException("Room must be between 1 and 10");
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            StudentCode = dto.StudentCode,
            FullName = dto.FullName,
            Department = dto.Department,
            Track = dto.Track,
            Year = dto.Year,
            Room = dto.Room,
            TeacherId = dto.TeacherId,
        };
        
        _Db.Students.Add(student); 
        await _Db.SaveChangesAsync(ct);
        return student;
    }

    public async Task<IReadOnlyCollection<Student>> SearchAsync(StudentFilterDto filter, CancellationToken ct = default)
    {
        var q = _Db.Students.AsQueryable();
        if (filter.Track is not null) q = q.Where(x => x.Track == filter.Track);
        if (filter.Year is not null) q = q.Where(x => x.Year == filter.Year);
        if (filter.Room is not null) q = q.Where(x => x.Room == filter.Room);
        if (filter.TeacherId is not null) q = q.Where(x => x.TeacherId == filter.TeacherId);

        if (!string.IsNullOrWhiteSpace(filter.Q))
        {
            q = q.Where(x=> x.StudentCode.Contains(filter.Q!) || x.FullName.Contains(filter.Q!));
        }

        return await q.OrderBy(x => x.StudentCode).Take(500).ToListAsync(ct);
    }
}