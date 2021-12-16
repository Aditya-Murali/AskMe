using System.ComponentModel.DataAnnotations;

namespace AskMe.Models.ViewModels.AnswerViewModels
{
    public class CreateAnswerViewModel
    {
        [Required]
        public string Statement { get; set; }

        [Required]
        public int QuestionId { get; set; }
    }
}
