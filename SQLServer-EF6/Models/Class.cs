using System;
using System.Collections.Generic;

namespace SQLServer_EF6.Models
{
    public partial class Class
    {
        public int Id { get; set; }
        public string? Classname { get; set; }
        public int? NumStudent { get; set; }

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
