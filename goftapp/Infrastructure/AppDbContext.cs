using goftapp.Entity;
using Microsoft.EntityFrameworkCore;

namespace goftapp.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
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
        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasIndex(x => x.Username).IsUnique();
            entity.Property(x => x.Role).HasConversion<int>(); // แปลง enum => int
            entity.HasOne(x => x.Student).WithOne().HasForeignKey<Users>(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Teacher).WithOne().HasForeignKey<Users>(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x=> x.CompanyRepOf).WithOne(x=> x.CompanyRepUser).HasForeignKey<Company>(x=> x.CompanyRepUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasIndex(x => x.TeacherCode).IsUnique().HasFilter("[TeacherCode] IS NOT NULL");
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(x => x.StudentCode).IsUnique();
            entity.Property(x => x.Track).HasConversion<int>();
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            entity.HasOne(x => x.Teacher).WithMany(x => x.Students).HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new { x.Track,x.Year,x.Room});
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<InternshipApplication>(entity =>
        {
            entity.Property(x => x.Status).HasConversion<int>();
            entity.HasOne(x => x.Student).WithMany().HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x=> x.Company).WithMany(x=> x.Applications).HasForeignKey(x=> x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.DecidedByCompanyRepUser).WithMany().HasForeignKey(x => x.DecidedByCompanyRepUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => new { x.CompanyId, x.Status });
            entity.HasIndex(x=> new {x.StudentId,x.CompanyId});
        });

        modelBuilder.Entity<DailyTrainingLog>(entity =>
        {
            entity.Property(x => x.Attendance).HasConversion<int>();
            entity.Property(x => x.Status).HasConversion<int>();

            entity.HasOne(x => x.Student).WithMany(x => x.DailyLogs).HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.ApprovedByCompanyRepUser).WithMany().HasForeignKey(x => x.ApprovedByCompanyRepUserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(x => x.ReviewedByTeacherUser).WithMany().HasForeignKey(x => x.ReviewedByTeacherUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(x => new { x.StudentId, x.Date }).IsUnique();
            entity.HasIndex(x => new { x.CompanyId, x.Date });

        });

        modelBuilder.Entity<TrainingPhoto>(entity =>
        {
            entity.Property(x => x.Url).IsRequired();
            entity.HasOne(x => x.DailyTrainingLog).WithMany(x => x.Photos).HasForeignKey(x => x.DailyTrainingLogId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}