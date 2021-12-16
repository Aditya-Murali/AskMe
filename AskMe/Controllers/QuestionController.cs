using AskMe.Models;
using AskMe.Models.ViewModels.QuestionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskMe.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AskMeDbContext _context;

        public QuestionController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AskMeDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Dashboard(int? id)
        {
            List<Question> questions = new List<Question>();

            if (id == null)
                questions = _context.Questions.ToList();
            else
                questions = _context.Questions.Where( q => q.category.CId == id ).ToList();

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
                        QId = q.QId
                    });
            }

            var qdvm = new QuestionDashboardViewModel
            {
                Questions = _questions,
                CategoryId = id
            };

            return View(qdvm);
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> History()
        {
            var currentUserId = await GetCurrentUserId();

            var questions = _context.Questions.Where( q => q.user.UserId == currentUserId ).ToList();
            List<QuestionViewModel> _questions = new List<QuestionViewModel>();

            if(questions.Count > 0)
            {
                foreach (Question q in questions)
                {
                    var answers = _context.Answers.Where(a => a.question.QId == q.QId).ToList();

                    _questions.Add(
                        new QuestionViewModel
                        {
                            QuestionStatement = q.Statement,
                            AnswerCount = answers.Count,
                            AnswerStatement = answers.Count > 0 ? answers.FirstOrDefault().Statement : null,
                            QId = q.QId
                        });
                }
            }    
           
            var qdvm = new QuestionDashboardViewModel
            {
                Questions = _questions
            };

            return View(qdvm);
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Details(int? qid)
        {
            if (qid == null)
                return NotFound();

            var question = _context.Questions.Where( q => q.QId == qid ).FirstOrDefault();
            var answers = _context.Answers.Where( a => a.question.QId == qid ).Include( a => a.user ).ToList();

            var qdvm = new QuestionDetailsViewModel
            {
                QuestionStatement = question.Statement,
                Ans = answers,
                QId = question.QId
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

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Delete(int qid)
        {
            if (qid == null)
                return NotFound();

            var qdvm = new QuestionDeleteViewModel
            {
               QuestionId = qid
            };

            return View(qdvm);
        }

        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Delete(QuestionDeleteViewModel qdvm)
        {
            if (ModelState.IsValid)
            {
                //Ensuring the question and its answers are deleted together.
                using (var dbTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var question = await _context.Questions.Where( q => q.QId == qdvm.QuestionId ).FirstOrDefaultAsync();
                        _context.Questions.Remove(question);

                        var answers = await _context.Answers.Where( a => a.question.QId == qdvm.QuestionId ).ToListAsync();
                        foreach(var a in answers)
                        {
                            _context.Answers.Remove(a);
                        }

                        await _context.SaveChangesAsync();
                        dbTransaction.Commit();

                        return RedirectToAction("History");
                    }
                    catch (Exception e)
                    {
                        dbTransaction.Rollback();
                        _logger.LogError(e, "Error during question deletion transaction.");
                        throw e;
                    }
                }
            }

            return BadRequest();
        }

        //private method for getting the current user id string
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user?.Id;
        }
    }
}
