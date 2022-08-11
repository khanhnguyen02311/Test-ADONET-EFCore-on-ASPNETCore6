using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SQLServer_EF6.Models
{
    public partial class GeneralDBContext : DbContext
    {
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public GeneralDBContext() {}

        public GeneralDBContext(DbContextOptions<GeneralDBContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Classname).HasMaxLength(50);
                entity.Property(e => e.NumStudent).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Birthdate).HasColumnType("smalldatetime");
                entity.Property(e => e.ClassId).HasColumnName("ClassID");
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(50);
                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_ClassID");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
