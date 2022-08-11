using System;
using System.Collections.Generic;

namespace SQLServer_EF6.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string Lastname { get; set; } = null!;
        public DateTime? Birthdate { get; set; }
        public int? ClassId { get; set; }
        
        public virtual Class? Class { get; set; }
    }
}
