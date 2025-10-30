using goftapp.DTOs;
using goftapp.Entity;

namespace goftapp.Services.Interfaces;

public interface IStudentService
{
    Task<Student> CreateStudent(CreateStudentDto dto ,CancellationToken ct = default);
    Task<IReadOnlyCollection<Student>> SearchAsync(StudentFilterDto filter, CancellationToken ct = default);
}