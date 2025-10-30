// DTOs/TeacherDtos.cs
namespace goftapp.DTOs;
public record CreateTeacherUserDto(string Username, string Password, string FullName, string? Department);