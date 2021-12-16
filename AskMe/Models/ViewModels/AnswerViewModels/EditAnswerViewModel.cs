using System.ComponentModel.DataAnnotations;

namespace AskMe.Models.ViewModels.AnswerViewModels
{
    public class EditAnswerViewModel
    {
        [Required]
        public string Statement { get; set; }

        [Required]
        public int AnswerId { get; set; }
    }
}
