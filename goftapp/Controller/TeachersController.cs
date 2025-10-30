using goftapp.DTOs;
using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/teachers")]
public class TeachersController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ICompanyService _companyService;

    public TeachersController(IStudentService studentService, ICompanyService companyService)
    {
        _studentService = studentService;
        _companyService = companyService;  
    }

    [HttpPost("students")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto)
    {
        var s = await _studentService.CreateStudent(dto);
        return Created($"/api/students/{s.Id}", s);
    }

    [HttpPost("companies")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
    {
        var c = await _companyService.CreateCompanyAsync(dto);
        return Created($"/api/companies/{c.Id}", c);
    }
}