using System;
using System.Collections.Generic;

namespace SQLServer_EF6.Models
{
    public partial class StudentModel
    {
        public int Id { get; set; } = 0;
        public string? Firstname { get; set; }
        public string Lastname { get; set; } = null!;
        public DateTime? Birthdate { get; set; }
        public int? ClassId { get; set; }
        public virtual ClassModel? Class { get; set; }
    }
}
