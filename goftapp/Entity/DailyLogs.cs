
namespace goftapp.Entity;

public class DailyTrainingLog
{
   
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public DateOnly Date { get; set; }    // 1 วัน/1 แถว ต่อ student (ทำ unique)
    public TimeOnly? CheckInAt { get; set; }
    public TimeOnly? CheckOutAt { get; set; }
    public AttendanceStatus Attendance { get; set; } = AttendanceStatus.Present;

    public double? CheckInLat { get; set; }
    public double? CheckInLng { get; set; }
    public double? CheckOutLat { get; set; }
    public double? CheckOutLng { get; set; }

    public string? Activities { get; set; }
    public string? Notes { get; set; }

    public DailyLogStatus Status { get; set; } = DailyLogStatus.Draft;
    public DateTime? SubmittedAtUtc { get; set; }

    public Guid? ApprovedByCompanyRepUserId { get; set; }
    public Users? ApprovedByCompanyRepUser { get; set; }
    public DateTime? ApprovedAtUtc { get; set; }
    public string? CompanyComment { get; set; }

    public Guid? ReviewedByTeacherUserId { get; set; }
    public Users? ReviewedByTeacherUser { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }

    public ICollection<TrainingPhoto> Photos { get; set; } = new List<TrainingPhoto>();
}

