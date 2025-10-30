using goftapp.DTOs;
using goftapp.Entity;

namespace goftapp.Services.Interfaces;

public interface IApplicationService
{
    Task<InternshipApplication> ApplyAsync(ApplyDto dto, CancellationToken ct = default);
    Task<InternshipApplication> DecideAsync(Guid applicationId, ApproveApplicationDto dto, Guid companyRepUserId, CancellationToken ct = default);
    Task<IReadOnlyList<InternshipApplication>> PendingForCompanyAsync(Guid companyId, CancellationToken ct = default);
}