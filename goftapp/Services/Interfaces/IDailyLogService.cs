using goftapp.DTOs;
using goftapp.Entity;

namespace goftapp.Services.Interfaces;

public interface IDailyLogService
{
    Task<DailyTrainingLog> CreateAsync(CreateDailyLogDto dto, CancellationToken ct = default);
    Task<DailyTrainingLog> UpdateAttendanceAsync(Guid logId, UpdateAttendanceDto dto, CancellationToken ct = default);
    Task<DailyTrainingLog> AddPhotoAsync(Guid logId, AddPhotoDto dto, CancellationToken ct = default);
    Task<DailyTrainingLog> SaveActivitiesAsync(Guid logId, SaveActivitiesDto dto, CancellationToken ct = default);
    Task<DailyTrainingLog> SubmitAsync(Guid logId, CancellationToken ct = default);
    Task<DailyTrainingLog> CompanyReviewAsync(Guid logId, ApproveDailyLogDto dto, Guid companyRepUserId, CancellationToken ct = default);
    Task<IReadOnlyList<DailyTrainingLog>> SearchAsync(DailyLogFilterDto filter, CancellationToken ct = default);
}