namespace AskMe.Models.ViewModels.QuestionViewModels
{
    public class QuestionDetailsViewModel
    {
        public int QId { get; set; }

        public string QuestionStatement { get; set; }

        public List<Answer> Ans { get; set; }
    }
}
