// DTOs/DailyLogDtos.cs
using System;
using goftapp.Entity;

namespace goftapp.DTOs;
public record CreateDailyLogDto(Guid StudentId, Guid CompanyId, DateOnly Date);
public record UpdateAttendanceDto(AttendanceStatus Status, TimeOnly? CheckInAt, TimeOnly? CheckOutAt,
    double? CheckInLat, double? CheckInLng, double? CheckOutLat, double? CheckOutLng);
public record AddPhotoDto(string Url, string? Caption);
public record SaveActivitiesDto(string? Activities, string? Notes);
public record SubmitDailyLogDto(); // ใช้เป็น marker
public record ApproveDailyLogDto(bool Approve, string? Comment);

public record DailyLogFilterDto(Guid? StudentId, Guid? CompanyId, DateOnly? Date, DailyLogStatus? Status, int Take = 200);