using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AskMe.Models
{
    public class Question
    {
        [Key]
        public int QId { get; set; }

        public string Statement { get; set; }

        [ForeignKey("UserId")]
        public virtual User user { get; set; }

        [ForeignKey("CId")]
        public virtual Category category { get; set; }
    }
}