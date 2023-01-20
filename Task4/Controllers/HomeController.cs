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
                    _signInManager.SignOutAsync();
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            return View();
        }

        [Authorize]
        public IActionResult UsersList()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsSignOutUserForcibly())
                    _signInManager.SignOutAsync();
                else
                    _repository.UpdateLastLoginDateUser(User.Identity.Name);
            }

            var users = _repository.GetAllUsers();
            List<UserModel> usersList = new List<UserModel>();
            foreach (var user in users)
                usersList.Add(user);
            
            return View(usersList);
        }

        [HttpPost]
        public IActionResult Block(List<UserModel> userModel)
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

            if (userModel.Count != 0)
                foreach (var user in userModel)
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

            return RedirectToAction("UsersList", "Home");
        }

        [HttpPost]
        public IActionResult UnBlock(List<UserModel> userModel)
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

            if (userModel.Count != 0)
                foreach (var user in userModel)
                    if (user.IsChecked)
                        _repository.UpdateBlockedStatus(user.UserName, false);

            return RedirectToAction("UsersList", "Home");
        }

        public IActionResult Delete(List<UserModel> userModel)
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

            if (userModel.Count != 0)
                foreach (var user in userModel)
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

            return RedirectToAction("UsersList", "Home");
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