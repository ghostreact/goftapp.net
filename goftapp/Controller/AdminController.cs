using goftapp.DTOs;
using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public AdminController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpPost("teacher")]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherUserDto dto,[FromQuery] Guid createdByAdminUserId, CancellationToken ct = default)
    {
        var (user, teacher) = await _teacherService.CreateTeacherAsync(dto, createdByAdminUserId);
        return Created($"/api/teacher/{teacher.Id}", new {userId = user.Id, teacherId = teacher.Id});
    }
}