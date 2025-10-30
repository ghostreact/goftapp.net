// DTOs/ApplicationDtos.cs
using System;
namespace goftapp.DTOs;
public record ApplyDto(Guid StudentId, Guid CompanyId, string? Position, DateOnly? StartDate, DateOnly? EndDate);
public record ApproveApplicationDto(bool Approve, string? RejectReason);