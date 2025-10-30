using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;   
    }
    
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] LevelClass? track, [FromQuery] int? year, [FromQuery] int? room,
        [FromQuery] Guid? teacherId, [FromQuery] string? q)
    {
        var res = await _studentService.SearchAsync(new StudentFilterDto(track, year, room, teacherId, q));
        return Ok(res);
    }
}