using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task4.Data;
using Task4.Data.Entities;
using Task4.Models;

namespace Task4.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IApplicationRepository _appRepository;

        public AccountController(SignInManager<ApplicationUser> signInManager, IApplicationRepository repository, 
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _appRepository = repository;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                _appRepository.UpdateLastLoginDateUser(User.Identity.Name);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid && !_appRepository.IsUserBlocked(model.UserName))
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,
                    false, false);
                if (result.Succeeded)
                {
                    _appRepository.UpdateLastLoginDateUser(model.UserName);
                    return RedirectToAction("UsersList", "Home");
                }
            }

            ModelState.AddModelError("", "Failed to login");
            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                _appRepository.UpdateLastLoginDateUser(User.Identity.Name);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser(model.UserName, model.Email);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result != IdentityResult.Success)
                {
                    ModelState.AddModelError("", result.ToString());
                    return View();
                }

                return await Login(new LoginModel { UserName = model.UserName, Password = model.Password });
            }

            ModelState.AddModelError("", "Failed to register.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (_appRepository.IsUserExist(User.Identity.Name) &&
                    !_appRepository.IsUserBlocked(User.Identity.Name))
                    _appRepository.UpdateLastLoginDateUser(User.Identity.Name);
                else
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
