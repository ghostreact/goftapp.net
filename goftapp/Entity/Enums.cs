namespace goftapp.Entity;

public enum UserRole { Admin, Teacher, Student, CompanyRep }

public enum ApplicationStatus { Pending, Approved, Rejected, Cancelled }

public enum DailyLogStatus { Draft, Submitted, ApprovedByCompany, RejectedByCompany, ReviewedByTeacher }

public enum AttendanceStatus { Present, Late, Leave, Absent }

// ปวส ปวช
public enum LevelClass
{
    Vocational,      // ปวช.
    HighVocational   // ปวส.
}

public static class StudentTrackConfig
{
    public static readonly Dictionary<LevelClass, int> MaxYears = new()
    {
        [LevelClass.HighVocational] = 2, // ปวส
        [LevelClass.Vocational] = 3 // ปวช
    };
    public static string Label(LevelClass level) => level == LevelClass.HighVocational ? "ปวส." : "ปวช.";

}
