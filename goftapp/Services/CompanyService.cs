using goftapp.DTOs;
using goftapp.Entity;
using goftapp.Infrastructure;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Services;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _Db;

    public CompanyService(AppDbContext db)
    {
        _Db = db;
    }
    
    public async Task<Company> CreateCompanyAsync(CreateCompanyDto dto, CancellationToken ct = default)
    {
        if (await _Db.Companies.AnyAsync(x => x.Name == dto.Name, ct))
        {
            throw new InvalidOperationException("Company already exists");
        }

        Users? req = null;
        if (!string.IsNullOrWhiteSpace(dto.CompanyRepUsername))
        {
            if (await _Db.Users.AnyAsync(x => x.Username == dto.CompanyRepUsername, ct))
            {
                throw new InvalidOperationException("Company User already exists");
            }

            req = new Users
            {
                Id = Guid.NewGuid(),
                Username = dto.CompanyRepUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.CompanyRepPassword),
                IsActive = true
            };
            _Db.Users.Add(req);
        }

        var com = new Company
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Address = dto.Address,
            ContactName = dto.ContactName,
            ContactPhone = dto.ContactPhone,
            CompanyRepUserId = req?.Id
        };
        _Db.Companies.Add(com);
        await _Db.SaveChangesAsync(ct);
        return com;
    }

    public async Task<IReadOnlyList<Company>> ListAsync(string? s, CancellationToken ct = default)
    {
        var search = _Db.Companies.AsQueryable();
        if (!string.IsNullOrWhiteSpace(s))
        {
            search = search.Where(x=> x.Name.Contains(s));
        }

        return await search.OrderBy(x => x.Name).Take(500).ToListAsync(ct);
    }
}