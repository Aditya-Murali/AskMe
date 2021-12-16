using AskMe.Models;
using AskMe.Models.ViewModels.AnswerViewModels;
using AskMe.Models.ViewModels.QuestionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AskMe.Controllers
{
    public class AnswerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AskMeDbContext _context;

        public AnswerController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AskMeDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> History()
        {
            var currentUserId = await GetCurrentUserId();

            var answers = _context.Answers.Where( a => a.user.UserId == currentUserId ).Include( a => a.question ).ToList();
            List<AnswerViewModel> answerHistory = new List<AnswerViewModel>();

            if (answers.Count > 0)
            {
                foreach (Answer a in answers)
                {
                    answerHistory.Add(
                        new AnswerViewModel
                        {
                            QuestionStatement = a.question.Statement,
                            AnswerStatement = a.Statement,
                            AId = a.AId
                        });
                }
            }

            var ahvm = new AnswerHistoryViewModel
            {
                AnswerHistory = answerHistory
            };

            return View(ahvm);
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Create(int? qid)
        {
            if(qid == null)
                return NotFound();

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

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Edit(int? aid)
        {
            if (aid == null)
                return NotFound();

            var _answer = _context.Answers.Where(a => a.AId == aid).FirstOrDefault();
            var cavm = new EditAnswerViewModel
            {
               AnswerId = aid.Value,
               Statement = _answer.Statement
            };

            return View(cavm);
        }

        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> EditAsync(EditAnswerViewModel eavm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _answer = _context.Answers.AsNoTracking().Where( a => a.AId == eavm.AnswerId ).Include( a => a.user ).Include( a => a.question ).FirstOrDefault();

                    var answer = new Answer()
                    {
                        AId = eavm.AnswerId,
                        Statement = eavm.Statement,
                        user = _answer.user,
                        question = _answer.question
                    };

                    _context.Answers.Update(answer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Success", "Home", new { act = "index", con = "home" });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while editing an answer");
                    throw e;
                }
            }

            return View(eavm);
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public IActionResult Delete(int aid)
        {
            if (aid == null)
                return NotFound();

            var advm = new AnswerDeleteViewModel
            {
                AnswerId = aid
            };

            return View(advm);
        }

        [HttpPost]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Delete(AnswerDeleteViewModel advm)
        {
            if (ModelState.IsValid)
            {
                var answer = await _context.Answers.Where( a => a.AId == advm.AnswerId ).FirstOrDefaultAsync();

                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
             
                return RedirectToAction("History");
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Status(int qid)
        {
            try
            {
                var currentUserId = await GetCurrentUserId();
                var _answer = _context.Answers.Where( a => (a.question.QId == qid) && ( a.user.UserId == currentUserId )).FirstOrDefault();

                if( _answer != null )
                    return RedirectToAction("Edit", "Answer", new { aid = _answer.AId});

                return RedirectToAction("Create", "Answer", new { qid = qid });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating an answer");
                throw e;
            }
        }

        //private method for getting the current user id string
        private async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return user?.Id;
        }
    }
}
