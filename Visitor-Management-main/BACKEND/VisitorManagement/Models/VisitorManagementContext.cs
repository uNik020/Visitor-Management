using Microsoft.EntityFrameworkCore;

namespace VisitorManagement.Models;

public partial class VisitorManagementContext : DbContext
{
    public VisitorManagementContext() { }

    public VisitorManagementContext(DbContextOptions<VisitorManagementContext> options)
        : base(options) { }

    public virtual DbSet<Admin> Admins { get; set; } = null!;
    public virtual DbSet<Department> Departments { get; set; } = null!;
    public virtual DbSet<Hosts> Hosts { get; set; } = null!;
    public virtual DbSet<Visit> Visits { get; set; } = null!;
    public virtual DbSet<Companion> Companions { get; set; } = null!;
    public virtual DbSet<Designation> Designation { get; set; } = null!;
    public virtual DbSet<Visitor> Visitors { get; set; } = null!;
    public virtual DbSet<PasswordResetRequest> PasswordResetRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId);
            entity.HasIndex(e => e.FullName).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(50);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.DesignationId);
            entity.Property(e => e.DesignationName).HasMaxLength(100);
        });

        modelBuilder.Entity<Hosts>(entity =>
        {
            entity.HasKey(e => e.HostId);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.ProfilePictureUrl).HasMaxLength(255);
            entity.Property(e => e.About).HasMaxLength(1000);

            entity.HasOne(d => d.Department)
                .WithMany(p => p.Hosts)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Hosts_Department");

            entity.HasOne(d => d.Designation)
    .WithMany(p => p.Hosts) // <-- Important
    .HasForeignKey(d => d.DesignationId)
    .HasConstraintName("FK_Hosts_Designation");

        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.VisitorId);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CompanyName).HasMaxLength(100);
            entity.Property(e => e.Purpose).HasMaxLength(255);
            entity.Property(e => e.Comment).HasMaxLength(500);
            entity.Property(e => e.IdProofType).HasMaxLength(50);
            entity.Property(e => e.IdProofNumber).HasMaxLength(100);
            entity.Property(e => e.LicensePlateNumber).HasMaxLength(50);
            entity.Property(e => e.PassCode).HasMaxLength(100);
            entity.Property(e => e.QrCodeData).HasMaxLength(255);
            entity.Property(e => e.PhotoUrl).HasColumnType("nvarchar(max)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.ExpectedVisitDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId);
            entity.Property(e => e.CheckInTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.VisitStatus).HasMaxLength(20).HasDefaultValue("CheckedIn");

            entity.HasOne(d => d.Host)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.HostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visits_Host");

            entity.HasOne(d => d.Visitor)
                .WithMany(p => p.Visits)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visits_Visitor");
        });

        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasKey(e => e.CompanionId);
            entity.Property(e => e.FullName).HasMaxLength(100);

            entity.HasOne(d => d.Visitor)
                .WithMany(p => p.Companions)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Companion_Visitor");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
