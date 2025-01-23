using CakeZone.BL.ViewModels.User;
using CakeZone.CORE.Enums;
using CakeZone.CORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CakeZone.MVC.Controllers
{
    public class AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager) : Controller
    {
        bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;


        [HttpGet]
        public IActionResult Register()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View();

            User user = new User
            {
                FullName = vm.FullName,
                UserName = vm.Username,
                Email = vm.Email,
            };

            var result = await _userManager.CreateAsync(user, vm.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

            if (!roleResult.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View();

            User? user = null!;

            if (vm.UsernameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);


            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);

            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "Username or password is wrong");
                if (result.IsLockedOut)
                    ModelState.AddModelError("", "Your account is blocked");

                ModelState.AddModelError("", "Username or password is wrong");

                return View();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                if (await _userManager.IsInRoleAsync(user, nameof(Roles.Admin)))
                {
                    return RedirectToAction("Index", new { controller = "Dashboard", Area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);


        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
