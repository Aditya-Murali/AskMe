using AskMe.Models;
using AskMe.Models.ViewModels.AnswerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AskMe.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AskMeDbContext _context;

        public AnswerController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AskMeDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Create(int? qid)
        {
            if(qid == null)
            {
                return NotFound();
            }

            var cavm = new CreateAnswerViewModel
            {
                QuestionId = qid.GetValueOrDefault()
            };

            return View(cavm);
        }

        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> CreateAsync(CreateAnswerViewModel cavm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUserId = await GetCurrentUserId();

                    var _user = _context.Users.Where(u => u.UserId == currentUserId).FirstOrDefault();
                    var _question = _context.Questions.Where(q => q.QId == cavm.QuestionId).FirstOrDefault();

                    var answer = new Answer()
                    {
                        Statement = cavm.Statement,
                        user = _user,
                        question = _question
                    };

                    // save the question to the DB
                    _context.Answers.Add(answer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Success", "Home", new { act = "index", con = "home" });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while creating an answer");
                    throw e;
                }
            }

            return View(cavm);
        }

        //private method for getting the current user id string
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user?.Id;
        }
    }
}
