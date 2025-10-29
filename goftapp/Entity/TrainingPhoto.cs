namespace goftapp.Entity;

public class TrainingPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DailyTrainingLogId { get; set; }
    public DailyTrainingLog DailyTrainingLog { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string? Caption { get; set; }
    public DateTime UploadedAtUtc { get; set; } = DateTime.UtcNow;
}