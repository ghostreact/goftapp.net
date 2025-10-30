using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace goftapp.Controller;

[ApiController]
[Route("api/logs")]
public class DailyLogsController : ControllerBase
{
    private readonly IDailyLogService _svc;
    public DailyLogsController(IDailyLogService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDailyLogDto dto)
        => Created("", await _svc.CreateAsync(dto));

    [HttpPut("{id:guid}/attendance")]
    public async Task<IActionResult> UpdateAttendance(Guid id, [FromBody] UpdateAttendanceDto dto)
        => Ok(await _svc.UpdateAttendanceAsync(id, dto));

    [HttpPost("{id:guid}/photos")]
    public async Task<IActionResult> AddPhoto(Guid id, [FromBody] AddPhotoDto dto)
        => Ok(await _svc.AddPhotoAsync(id, dto));

    [HttpPut("{id:guid}/activities")]
    public async Task<IActionResult> SaveActivities(Guid id, [FromBody] SaveActivitiesDto dto)
        => Ok(await _svc.SaveActivitiesAsync(id, dto));

    [HttpPut("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id, [FromBody] SubmitDailyLogDto _)
        => Ok(await _svc.SubmitAsync(id));

    [HttpPut("{id:guid}/company-review")]
    public async Task<IActionResult> CompanyReview(Guid id, [FromBody] ApproveDailyLogDto dto, [FromQuery] Guid companyRepUserId)
        => Ok(await _svc.CompanyReviewAsync(id, dto, companyRepUserId));

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] Guid? studentId, [FromQuery] Guid? companyId,
        [FromQuery] DateOnly? date, [FromQuery] DailyLogStatus? status, [FromQuery] int take = 200)
        => Ok(await _svc.SearchAsync(new DailyLogFilterDto(studentId, companyId, date, status, take)));
}