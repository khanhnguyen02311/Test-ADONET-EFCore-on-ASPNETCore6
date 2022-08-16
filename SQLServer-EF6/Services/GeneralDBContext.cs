using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SQLServer_EF6.Models;

namespace SQLServer_EF6.Services
{
    public partial class GeneralDBContext : DbContext
    {
        public virtual DbSet<ClassModel> Classes { get; set; } = null!;
        public virtual DbSet<StudentModel> Students { get; set; } = null!;
        public GeneralDBContext() {}

        public GeneralDBContext(DbContextOptions<GeneralDBContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassModel>(entity =>
            {
                entity.ToTable("Class");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Classname).HasMaxLength(50);
                entity.Property(e => e.NumStudent).HasDefaultValueSql("((0))");
            });
            modelBuilder.Entity<StudentModel>(entity =>
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

        
        public List<StudentModel> getAllStudent()
        {
            // Include: load the data with the foreign key's values
            return Students.Include(s => s.Class).OrderBy(s => s.Id).ToList();
        }

        public List<ClassModel> getAllClass()
        {
            return Classes.OrderBy(c => c.Id).ToList();
        }

        public ClassModel getClassById(int id)
        {
            var c = Classes.Find(id);
            return c ?? new ClassModel();
        }

        public StudentModel getStudentById(int id)
        {
            var s = Students.Find(id);
            return s ?? new StudentModel();
        }
    }
}
