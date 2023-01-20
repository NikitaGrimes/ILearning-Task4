using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task4.Data;
using Task4.Data.Entities;
using Task4.Models;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationRepository _repository;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IApplicationRepository repository, SignInManager<ApplicationUser> signInManager)
        {
            _repository = repository;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            return View();
        }

        [Authorize]
        public IActionResult Users()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            var users = _repository.GetAllUsers();
            List<UserModel> usersNodel = new List<UserModel>();
            foreach (var user in users)
                usersNodel.Add(user);
            
            return View(usersNodel);
        }

        [HttpPost]
        public IActionResult Block(List<UserModel> usersModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            if (usersModel.Count != 0)
                foreach (var user in usersModel)
                    if (user.IsChecked)
                        _repository.UpdateBlockedStatus(user.UserName, true);

            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            return RedirectToAction("Users", "Home");
        }

        [HttpPost]
        public IActionResult UnBlock(List<UserModel> usersModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            if (usersModel.Count != 0)
                foreach (var user in usersModel)
                    if (user.IsChecked)
                        _repository.UpdateBlockedStatus(user.UserName, false);

            return RedirectToAction("Users", "Home");
        }

        public IActionResult Delete(List<UserModel> usersModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            if (usersModel.Count != 0)
                foreach (var user in usersModel)
                    if (user.IsChecked)
                        _repository.DeleteUser(user.UserName);

            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                {
                    _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            return RedirectToAction("Users", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsSignOutUserForcibly()
        {
            return !(_repository.IsUserExist(User.Identity.Name) &&
                    !_repository.IsUserBlocked(User.Identity.Name));
        }

    }
}