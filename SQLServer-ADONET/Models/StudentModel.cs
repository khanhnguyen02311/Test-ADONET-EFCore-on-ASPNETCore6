using System.ComponentModel.DataAnnotations;

namespace SQLServer_ADONET.Models
{
    public class StudentModel
    {
        [Key]
        public int ID { get; set; }
        public int? ClassID { get; set; }
        [Required]
        public string Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Classname { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
