using goftapp.Entity;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<InternshipApplication> Applications => Set<InternshipApplication>();
    public DbSet<DailyTrainingLog> DailyLogs => Set<DailyTrainingLog>();
    public DbSet<TrainingPhoto> Photos => Set<TrainingPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========= Users =========
        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasIndex(x => x.Username).IsUnique();
            entity.Property(x => x.Username).IsRequired().HasMaxLength(100);
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.Property(x => x.Role).HasConversion<int>();

            // โปรไฟล์ 1:1 (ห้าม Cascade เพื่อกัน multiple cascade paths)
            entity.HasOne(x => x.Student).WithOne()
                  .HasForeignKey<Users>(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Teacher).WithOne()
                  .HasForeignKey<Users>(x => x.TeacherId)
                  .OnDelete(DeleteBehavior.Restrict);

            // ผู้ใช้ที่เป็นตัวแทนบริษัท (Company.CompanyRepUserId เป็น FK ชี้กลับมา)
            entity.HasOne(x => x.CompanyRepOf)
                  .WithOne(x => x.CompanyRepUser)
                  .HasForeignKey<Company>(x => x.CompanyRepUserId)
                  .OnDelete(DeleteBehavior.SetNull); // ต้องเป็น Guid? ใน Company
        });

        // ========= Teacher =========
        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Department).HasMaxLength(100);
            entity.HasIndex(x => x.TeacherCode)
                  .IsUnique()
                  .HasFilter("[TeacherCode] IS NOT NULL");

            // ถ้ามีฟิลด์ CreatedByAdminUserId (FK → Users) ให้กัน Cascade ไว้
            // ปรับให้ตรงกับพร็อพจริงในคลาส Teacher ถ้ามี Navigation
            // entity.HasOne<Users>()
            //       .WithMany()
            //       .HasForeignKey(x => x.CreatedByAdminUserId)
            //       .OnDelete(DeleteBehavior.Restrict);
        });

        // ========= Student =========
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(x => x.StudentCode).IsRequired().HasMaxLength(50);
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Department).HasMaxLength(100);

            entity.HasIndex(x => x.StudentCode).IsUnique();

            // Track/Year/Room
            entity.Property(x => x.Track).HasConversion<int>();
            entity.HasIndex(x => new { x.Track, x.Year, x.Room });

            // ครูประจำห้อง (กัน Cascade)
            entity.HasOne(x => x.Teacher)
                  .WithMany(x => x.Students)
                  .HasForeignKey(x => x.TeacherId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ========= Company =========
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Address).HasMaxLength(300);
            entity.Property(x => x.ContactName).HasMaxLength(100);
            entity.Property(x => x.ContactPhone).HasMaxLength(50);

            entity.HasIndex(x => x.Name).IsUnique();
            // CompanyRepUserId เป็น Guid? และตั้งค่าใน Users ด้านบนแล้ว
        });

        // ========= InternshipApplication =========
        modelBuilder.Entity<InternshipApplication>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<int>();
            entity.Property(x => x.Position).HasMaxLength(200);

            // map DateOnly -> date
            entity.Property(x => x.StartDate).HasColumnType("date");
            entity.Property(x => x.EndDate).HasColumnType("date");

            entity.HasOne(x => x.Student)
                  .WithMany()
                  .HasForeignKey(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Company)
                  .WithMany(x => x.Applications)
                  .HasForeignKey(x => x.CompanyId)
                  .OnDelete(DeleteBehavior.Cascade);

            // ผู้แทนบริษัทที่ตัดสินผล (NoAction/SetNull ปลอดภัยกว่า)
            entity.HasOne(x => x.DecidedByCompanyRepUser)
                  .WithMany()
                  .HasForeignKey(x => x.DecidedByCompanyRepUserId)
                  .OnDelete(DeleteBehavior.SetNull); // ต้องเป็น Guid? ในโมเดล

            entity.HasIndex(x => new { x.CompanyId, x.Status });
            entity.HasIndex(x => new { x.StudentId, x.CompanyId });
        });

        // ========= DailyTrainingLog =========
        modelBuilder.Entity<DailyTrainingLog>(entity =>
        {
            entity.Property(x => x.Attendance).HasConversion<int>();
            entity.Property(x => x.Status).HasConversion<int>();

            // map DateOnly/TimeOnly ให้ชัด
            entity.Property(x => x.Date).HasColumnType("date");
            entity.Property(x => x.CheckInAt).HasColumnType("time");
            entity.Property(x => x.CheckOutAt).HasColumnType("time");

            // ความสัมพันธ์หลัก
            entity.HasOne(x => x.Student)
                  .WithMany(x => x.DailyLogs)
                  .HasForeignKey(x => x.StudentId)
                  .OnDelete(DeleteBehavior.Cascade);   // เส้น cascade เดียวที่อนุญาต

            entity.HasOne(x => x.Company)
                  .WithMany()
                  .HasForeignKey(x => x.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);

            // ชี้ไป Users สองเส้น: ห้าม Cascade (กัน multiple cascade paths)
            entity.HasOne(x => x.ApprovedByCompanyRepUser)
                  .WithMany()
                  .HasForeignKey(x => x.ApprovedByCompanyRepUserId)
                  .OnDelete(DeleteBehavior.NoAction); // หรือ .NoAction

            entity.HasOne(x => x.ReviewedByTeacherUser)
                  .WithMany()
                  .HasForeignKey(x => x.ReviewedByTeacherUserId)
                  .OnDelete(DeleteBehavior.NoAction); // หรือ .NoAction

            // Indexes
            entity.HasIndex(x => new { x.StudentId, x.Date }).IsUnique(); // 1 วัน/คน = 1 แถว
            entity.HasIndex(x => new { x.CompanyId, x.Date });
        });

        // ========= TrainingPhoto =========
        modelBuilder.Entity<TrainingPhoto>(entity =>
        {
            entity.Property(x => x.Url).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Caption).HasMaxLength(300);

            entity.HasOne(x => x.DailyTrainingLog)
                  .WithMany(x => x.Photos)
                  .HasForeignKey(x => x.DailyTrainingLogId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
