using goftapp.DTOs;
using goftapp.Entity;

namespace goftapp.Services.Interfaces;

public interface ITeacherService
{
    Task<(Users user, Teacher teacher)> CreateTeacherAsync(CreateTeacherUserDto dto, Guid createdByAdminUserId,
        CancellationToken ct = default);
}