using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AskMe.Models
{
    public class Answer
    {
        [Key]
        public int AId { get; set; }

        public string Statement { get; set; }

        [ForeignKey("QId")]
        public virtual Question question { get; set; }

        public int QId{get; set;}

        [ForeignKey("UserId")]
        public virtual User user { get; set; }

        public string UserId{get; set;}
    }
}