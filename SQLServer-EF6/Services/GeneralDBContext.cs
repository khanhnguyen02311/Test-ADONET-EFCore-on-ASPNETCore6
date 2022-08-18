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
            return Students.Include(e => e.Class).OrderBy(e => e.Id).ToList();
        }

        public List<ClassModel> getAllClass()
        {
            return Classes.OrderBy(e => e.Id).ToList();
        }

        public ClassModel getClassById(int id)
        {
            var c = Classes.Include(e => e.Students).SingleOrDefault(e => e.Id == id);
            return c ?? new ClassModel();
        }

        public StudentModel getStudentById(int id)
        {
            var s = Students.Include(e => e.Class).SingleOrDefault(e => e.Id == id);
            return s ?? new StudentModel();
        }

        public List<StudentModel> getStudentsByClass(int? classid)
        {
            return Students.Where(e => e.ClassId == classid).ToList();
        }

        public void updateStudentClass(int id, int? classid)
        {
            var students = Students.Where(e => e.Id == id);
            foreach (var student in students) student.ClassId = classid;
            this.SaveChanges();
        }
    }
}
