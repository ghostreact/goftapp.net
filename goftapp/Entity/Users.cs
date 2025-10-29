using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace goftapp.Entity;

[BsonIgnoreExtraElements]
public class Users
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public UserRole Role { get; set; }
    public bool IsActive { get; set; } = true;

    // โปรไฟล์ (หนึ่งในสาม)
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }

    public Guid? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public Guid? CompanyRepId { get; set; }
    public Company? CompanyRepOf { get; set; } // Company ที่ user เป็นตัวแทน (1:1 optional)

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}