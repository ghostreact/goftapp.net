namespace goftapp.Entity;

public class Teacher
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = default!;
    public string? Department { get; set; }
    public string? TeacherCode { get; set; } // code ภายในโรงเรียน (unique ได้)

    public Guid CreatedByAdminUserId { get; set; }  // FK ไป User(Admin) ก็ได้หรือเก็บเปล่าก็ได้
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}