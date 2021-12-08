using System.ComponentModel.DataAnnotations;

namespace AskMe.Models
{
    public class Category
    {
        [Key]
        public int CId { get; set; }

        public string Name { get; set; }
    }
}
