using goftapp.DTOs;
using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/applications")]

public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }
    
    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyDto dto)
    {
        var app = await _applicationService.ApplyAsync(dto);
        return Created($"/api/applications/{app.Id}", app);
    }
    
    [HttpPut("{id:guid}/decide")]
    public async Task<IActionResult> Decide(Guid id, [FromBody] ApproveApplicationDto dto, [FromQuery] Guid companyRepUserId)
    {
        var app = await _applicationService.DecideAsync(id, dto, companyRepUserId);
        return Ok(app);
    }
    
    [HttpGet("pending/{companyId:guid}")]
    public async Task<IActionResult> PendingForCompany(Guid companyId)
        => Ok(await _applicationService.PendingForCompanyAsync(companyId));
}