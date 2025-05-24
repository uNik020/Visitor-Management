using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VisitorManagement.Models;

public partial class VisitorManagementContext : DbContext
{
    public VisitorManagementContext()
    {
    }

    public VisitorManagementContext(DbContextOptions<VisitorManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Hosts> Hosts { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    public virtual DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admins__719FE4E892EA49A9");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E457A42CF5").IsUnique();
            entity.Property(e => e.AdminId).HasColumnName("AdminId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD623CB80C");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Hosts>(entity =>
        {
            entity.HasKey(e => e.HostId).HasName("PK__Hosts__08D4870CF19A5379");

            entity.Property(e => e.HostId).HasColumnName("HostID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Department).WithMany(p => p.Hosts)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Hosts__Departmen__440B1D61");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visits__4D3AA1BE72B5A18D");

            entity.Property(e => e.VisitId).HasColumnName("VisitID");
            entity.Property(e => e.CheckInTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.HostId).HasColumnName("HostID");
            entity.Property(e => e.VisitStatus)
                .HasMaxLength(20)
                .HasDefaultValue("CheckedIn");
            entity.Property(e => e.VisitorId).HasColumnName("VisitorID");

            entity.HasOne(d => d.Host).WithMany(p => p.Visits)
                .HasForeignKey(d => d.HostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visits__HostID__49C3F6B7");

            entity.HasOne(d => d.Visitor).WithMany(p => p.Visits)
                .HasForeignKey(d => d.VisitorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Visits__VisitorI__48CFD27E");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.VisitorId).HasName("PK__Visitors__B121AFA86A481A25");

            entity.Property(e => e.VisitorId).HasColumnName("VisitorID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            //entity.Property(e => e.PhotoPath).HasMaxLength(255);
            entity.Property(e => e.Purpose).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
