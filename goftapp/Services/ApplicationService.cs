using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Infrastructure;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public class ApplicationService : IApplicationService
{
    private readonly AppDbContext _db;

    public ApplicationService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<InternshipApplication> ApplyAsync(ApplyDto dto, CancellationToken ct = default)
    {
        // ตรวจไม่ให้ Approved ซ้อน (อย่างง่าย)
        var hasApproved = await _db.Applications.AnyAsync(a =>
            a.StudentId == dto.StudentId && a.Status == ApplicationStatus.Approved, ct);
        if (hasApproved) throw new InvalidOperationException("Student already has an approved application.");

        var app = new InternshipApplication
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            CompanyId = dto.CompanyId,
            Position = dto.Position,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = ApplicationStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };
        _db.Applications.Add(app);
        await _db.SaveChangesAsync(ct);
        return app;
    }

    public async Task<InternshipApplication> DecideAsync(Guid applicationId, ApproveApplicationDto dto, Guid companyRepUserId, CancellationToken ct = default)
    {
        var app = await _db.Applications.FirstOrDefaultAsync(x => x.Id == applicationId, ct)
                  ?? throw new KeyNotFoundException("Application not found.");

        app.DecidedByCompanyRepUserId = companyRepUserId;
        app.DecidedAtUtc = DateTime.UtcNow;
        app.Status = dto.Approve ? ApplicationStatus.Approved : ApplicationStatus.Rejected;
        app.RejectReason = dto.Approve ? null : dto.RejectReason;

        await _db.SaveChangesAsync(ct);
        return app;
    }

    public async Task<IReadOnlyList<InternshipApplication>> PendingForCompanyAsync(Guid companyId, CancellationToken ct = default)
    {
        
        return await _db.Applications
            .Where(x=>x.CompanyId == companyId && x.Status == ApplicationStatus.Pending)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);
    }
}