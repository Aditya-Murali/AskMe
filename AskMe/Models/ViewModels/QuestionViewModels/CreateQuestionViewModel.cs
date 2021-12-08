using System.ComponentModel.DataAnnotations;

namespace AskMe.Models.ViewModels.QuestionViewModels
{
    public class CreateQuestionViewModel
    {
        [Required]
        public string Statement { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<Category>? Categories { get; set; }
    }
}
