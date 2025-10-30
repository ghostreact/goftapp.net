// DTOs/StudentDtos.cs
using goftapp.Entity;
namespace goftapp.DTOs;
public record CreateStudentDto(
    string StudentCode, 
    string FullName, 
    string? Department, 
    LevelClass Track, 
    int Year, 
    int Room, 
    Guid TeacherId
    );
public record StudentFilterDto(LevelClass? Track, int? Year, int? Room, Guid? TeacherId, string? Q);