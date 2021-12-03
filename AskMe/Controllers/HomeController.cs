using AskMe.Models;
using AskMe.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AskMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AskMeDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AskMeDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                LoginViewModel lvm = new LoginViewModel();
                return View(lvm);
            }
            catch (Exception e)
            {
                // Log and throw error
                _logger.LogError(e, "Error while Login ");
                throw e;
            }
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(LoginViewModel lvm)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, lockoutOnFailure: false);
                if(result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(lvm.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();

                    if (role.Equals("Consumer"))
                    {
                        return RedirectToAction("Success", "Home", new { act = "index", con = "home" });
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
               
                return View(lvm);
            }
            catch (Exception e)
            {
                // Log and throw error
                _logger.LogError(e, "Error while Login ");
                throw e;
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                RegisterViewModel rvm = new RegisterViewModel();
                return View(rvm);
            }
            catch (Exception e)
            {
                // Log and throw error
                _logger.LogError(e, "Error while registering a new user");
                throw e;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser { UserName = rvm.Email, Email = rvm.Email };
                    var result = await _userManager.CreateAsync(user, rvm.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "consumer");

                        User _user = new User
                        {
                            UserId = user.Id,
                            FirstName = rvm.FirstName,
                            LastName = rvm.LastName
                        };

                        _context.Users.Add(_user);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Success", "Home", new {act = "index", con = "home"});
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Error while registering a new user");
                    throw e;
                }
            }

            return View();
        }

        public IActionResult Success(string act, string con)
        {
            SuccessViewModel svm = new SuccessViewModel
            {
                Action = act,
                Controller = con
            };

            return View(svm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void AddErrors(IdentityResult result)
        {
            try
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception e)
            {
                // Log and throw error
                _logger.LogError(e, "Error while adding errors");
                throw e;
            }
        }
    }
}