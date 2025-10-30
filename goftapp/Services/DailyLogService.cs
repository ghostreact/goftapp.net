using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Infrastructure;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public class DailyLogService : IDailyLogService
{
    private readonly AppDbContext _Db;
    public DailyLogService(AppDbContext db)
    {
        _Db = db;
    }
    public async Task<DailyTrainingLog> AddPhotoAsync(Guid logId, AddPhotoDto dto, CancellationToken ct = default)
    {
        var log = await _Db.DailyLogs.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == logId, ct)
                   ?? throw new KeyNotFoundException("Log not found.");

        if (log.Photos.Count >= 2) throw new InvalidOperationException("Max 2 photos per day.");

        _Db.Photos.Add(new TrainingPhoto
        {
            Id = Guid.NewGuid(),
            DailyTrainingLogId = log.Id,
            Url = dto.Url,
            Caption = dto.Caption,
            UploadedAtUtc = DateTime.UtcNow
        });

        await _Db.SaveChangesAsync(ct);
        return log;
    }

    public async  Task<DailyTrainingLog> CompanyReviewAsync(Guid logId, ApproveDailyLogDto dto, Guid companyRepUserId, CancellationToken ct = default)
    {
        var log = await _Db.DailyLogs.FirstOrDefaultAsync(x => x.Id == logId, ct)
                 ?? throw new KeyNotFoundException("Log not found.");
        log.CompanyComment = dto.Comment;
        log.ApprovedByCompanyRepUserId = companyRepUserId;
        log.ApprovedAtUtc = DateTime.UtcNow;
        log.Status = dto.Approve ? DailyLogStatus.ApprovedByCompany : DailyLogStatus.RejectedByCompany;
        await _Db.SaveChangesAsync(ct);
        return log;
    }

    public async Task<DailyTrainingLog> CreateAsync(CreateDailyLogDto dto, CancellationToken ct = default)
    {
        var exists =await _Db.DailyLogs.AnyAsync(x => x.StudentId == dto.StudentId && x.Date == dto.Date, ct);
        if (exists)
            throw new InvalidOperationException("Daily log for this student on this date already exists.");

        var log = new DailyTrainingLog
        {
            Id = Guid.NewGuid(),
            StudentId = dto.StudentId,
            CompanyId = dto.CompanyId,
            Date = dto.Date,
            Status = DailyLogStatus.Draft
        };
        _Db.DailyLogs.Add(log);
        await _Db.SaveChangesAsync(ct);
        return log;
    }

    public async Task<DailyTrainingLog> SaveActivitiesAsync(Guid logId, SaveActivitiesDto dto, CancellationToken ct = default)
    {
        var log = await _Db.DailyLogs.FirstOrDefaultAsync(x => x.Id == logId, ct)
                  ?? throw new KeyNotFoundException("Log not found.");
        log.Activities = dto.Activities;
        log.Notes = dto.Notes;
        await _Db.SaveChangesAsync(ct);
        return log;
    }

    public async Task<IReadOnlyList<DailyTrainingLog>> SearchAsync(DailyLogFilterDto filter, CancellationToken ct = default)
    {
        var q = _Db.DailyLogs.AsQueryable();
        if (filter.StudentId is not null) q = q.Where(x => x.StudentId == filter.StudentId);
        if (filter.CompanyId is not null) q = q.Where(x => x.CompanyId == filter.CompanyId);
        if (filter.Date is not null) q = q.Where(x => x.Date == filter.Date);
        if (filter.Status is not null) q = q.Where(x => x.Status == filter.Status);
        return await q.OrderByDescending(x => x.Date).Take(Math.Clamp(filter.Take, 1, 1000)).ToListAsync(ct);
    }

    public async Task<DailyTrainingLog> SubmitAsync(Guid logId, CancellationToken ct = default)
    {
        var log = await _Db.DailyLogs.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == logId, ct)
                  ?? throw new KeyNotFoundException("Log not found.");
        if (log.Photos.Count != 2) throw new InvalidOperationException("Must attach exactly 2 photos.");
        log.Status = DailyLogStatus.Submitted;
        log.SubmittedAtUtc = DateTime.UtcNow;
        await _Db.SaveChangesAsync(ct);
        return log;
    }

    public async Task<DailyTrainingLog> UpdateAttendanceAsync(Guid logId, UpdateAttendanceDto dto, CancellationToken ct = default)
    {
        var log = await _Db.DailyLogs.FirstOrDefaultAsync(x => x.Id == logId, ct) ?? throw new KeyNotFoundException("Log not Found");

        log.Attendance = dto.Status;
        if(dto.CheckInAt is not null)
            log.CheckInAt = dto.CheckInAt;

        if(dto.CheckOutAt is not null)
            log.CheckOutAt = dto.CheckOutAt;

        log.CheckInLat = dto.CheckInLat;
        log.CheckOutLat = dto.CheckOutLat;
        log.CheckOutLng = dto.CheckOutLng;
        log.CheckInLng = dto.CheckInLng;

        await _Db.SaveChangesAsync(ct);
        return log;
    }
}