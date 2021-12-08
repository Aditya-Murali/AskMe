using AskMe.Models;
using AskMe.Models.ViewModels.QuestionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskMe.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AskMeDbContext _context;

        public QuestionController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AskMeDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Dashboard()
        {
            var questions = _context.Questions.ToList();
            List<QuestionViewModel> _questions = new List<QuestionViewModel>();
            
            foreach(Question q in questions)
            {
                var answers = _context.Answers.Where( a => a.question.QId == q.QId ).ToList();

                _questions.Add(
                    new QuestionViewModel
                    {
                        QuestionStatement = q.Statement,
                        AnswerCount = answers.Count,
                        AnswerStatement = answers.Count > 0 ? answers.FirstOrDefault().Statement : null,
                    });
            }

            var qdvm = new QuestionDashboardViewModel
            {
                Questions = _questions
            };

            return View(qdvm);
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Create()
        {
            var cqvm = new CreateQuestionViewModel
            {
                Categories = _context.Categories.ToList()
            };

            return View(cqvm);
        }

        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> CreateAsync(CreateQuestionViewModel cqvm)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var currentUserId = await GetCurrentUserId();

                    var _user = _context.Users.Where( u => u.UserId == currentUserId ).FirstOrDefault();
                    var _category = _context.Categories.Where( c => c.CId == cqvm.CategoryId ).FirstOrDefault();

                    var question = new Question()
                    {
                        Statement = cqvm.Statement,
                        user = _user,
                        category = _category
                    };

                    // save the question to the DB
                    _context.Questions.Add(question);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Success", "Home", new { act = "index", con = "home" });
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Error while creating a question");
                    throw e;
                }
            }

            return View(cqvm);
        }

        //private method for getting the current user id string
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user?.Id;
        }

        //private method for getting the current user email string
        private async Task<string> GetCurrentUserEmail()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user?.Email;
        }
    }
}
