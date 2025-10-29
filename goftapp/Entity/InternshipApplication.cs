namespace goftapp.Entity;

public class InternshipApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string? RejectReason { get; set; }
    public string? Position { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public Guid? DecidedByCompanyRepUserId { get; set; }
    public Users? DecidedByCompanyRepUser { get; set; }
    public DateTime? DecidedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}