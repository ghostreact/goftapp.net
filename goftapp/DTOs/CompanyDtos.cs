// DTOs/CompanyDtos.cs
namespace goftapp.DTOs;
public record CreateCompanyDto(
    string Name, 
    string? Address, 
    string? ContactName, 
    string? ContactPhone,
    string? CompanyRepUsername, 
    string? CompanyRepPassword
    );