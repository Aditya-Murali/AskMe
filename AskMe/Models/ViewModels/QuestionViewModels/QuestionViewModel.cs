namespace AskMe.Models.ViewModels.QuestionViewModels
{
    public class QuestionViewModel
    {
        public int QId { get; set; }

        public string QuestionStatement { get; set; }

        public string? AnswerStatement { get; set; }

        public int AnswerCount { get; set; }
    }
}
