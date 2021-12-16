namespace AskMe.Models.ViewModels.QuestionViewModels
{
    public class QuestionDashboardViewModel
    {
        public List<QuestionViewModel> Questions { get; set; }

        public int? CategoryId { get; set; }
    }
}
