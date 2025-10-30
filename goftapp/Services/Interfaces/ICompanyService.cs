using goftapp.DTOs;
using goftapp.Entity;

namespace goftapp.Services.Interfaces;

public interface ICompanyService
{
    Task<Company> CreateCompanyAsync(CreateCompanyDto dto,CancellationToken ct = default);
    Task<IReadOnlyList<Company>> ListAsync (string? s, CancellationToken ct = default);
}