using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/companies")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _svc;

    public CompaniesController(ICompanyService svc)
    {
        _svc = svc;
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? q)
        => Ok(await _svc.ListAsync(q));
}