using System;
using System.Collections.Generic;

namespace SQLServer_EF6.Models
{
    public partial class ClassModel
    {
        public int Id { get; set; } = 0;
        public string? Classname { get; set; }
        public int? NumStudent { get; set; }

        public virtual ICollection<StudentModel> Students { get; set; }
    }
}
