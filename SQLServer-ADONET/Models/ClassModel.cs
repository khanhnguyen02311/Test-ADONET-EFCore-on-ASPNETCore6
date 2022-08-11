using System.ComponentModel.DataAnnotations;

namespace SQLServer_ADONET.Models
{
    public class ClassModel
    {
        [Key]
        public int ID { get; set; }
        public string Classname { get; set; }
        public int NumStudent { get; set; }
    }
}
