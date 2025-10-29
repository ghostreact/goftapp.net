namespace goftapp.Entity;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string StudentCode { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? Department { get; set; }

    public LevelClass Track { get; set; } = LevelClass.HighVocational;
    public int Year { get; set; } = 1;  // ปวช. 1–3, ปวส. 1–2
    public int Room { get; set; } = 1;  // 1–20

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;

    public ICollection<DailyTrainingLog> DailyLogs { get; set; } = new List<DailyTrainingLog>();
}