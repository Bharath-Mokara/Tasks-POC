using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FluentMigratorTask.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source = ASPLAP1680 ;Initial Catalog = UserSampleDB ;Integrated Security = SSPI ; Trusted_Connection = True ;TrustServerCertificate = True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo");

            entity.HasIndex(e => e.Version, "UC_Version")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
