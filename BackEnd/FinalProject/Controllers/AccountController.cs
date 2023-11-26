using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.Services.Interfaces;
using FinalProject.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly GoogleCaptchaService _googleCaptchaService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IFileService fileService, RoleManager<IdentityRole> roleManager, GoogleCaptchaService googleCaptchaService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _fileService = fileService;
            _roleManager = roleManager;
            _googleCaptchaService = googleCaptchaService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            AppUser user = new()
            {
                Fullname = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string? link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token },
                Request.Scheme, Request.Host.ToString());
            if (link ==null)
            {
                return NotFound();
            }

            string body = string.Empty;

            string path = "wwwroot/Template/Verify.html";
            body = _fileService.ReadFile(path, body);


            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", user.Fullname);


            string subject = "Verify Email";
            _emailService.Send(user.Email, subject, body);

            return RedirectToAction(nameof(VerifyEmail));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return NotFound();

            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);

            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

       

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AppUser? user = await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);
            var captchaResult = await _googleCaptchaService.VerifyToken(loginVM.Token);
            if (!captchaResult)
            {
                ModelState.AddModelError("Token", "Please verify captcha");
                return View(loginVM);
            }
            user ??= await _userManager.FindByNameAsync(loginVM.UserNameorEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "Email or password wrong");
                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password wrong!");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            if (!ModelState.IsValid) return NotFound();

            AppUser? exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (exsistUser == null)
            {
                ModelState.AddModelError("Email", "Email isn't found");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

            string? link = Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token },
                Request.Scheme, Request.Host.ToString());



            string body = string.Empty;

            string path = "wwwroot/Template/Change.html";
            body = _fileService.ReadFile(path, body);


            body = body.Replace("{{link}}", link);
            body = body.Replace("{{Fullname}}", exsistUser.Fullname);


            string subject = "Verify Email";
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            _emailService.Send(exsistUser.Email, subject, body);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            return RedirectToAction(nameof(VerifyEmail));

        }

        public IActionResult ResetPassword(string userId, string token)
        {
            ResetPasswordVM resetPassword = new()
            {
                UserId = userId,
                Token = token
            };
            return View(resetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid) return View();



            AppUser? exsistUser = await _userManager.FindByIdAsync(resetPassword.UserId);



#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            bool chekPassword = await _userManager.VerifyUserTokenAsync(exsistUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPassword.Token);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

            if (!chekPassword) return Content("Error");


            if (exsistUser == null) return NotFound();

            if (await _userManager.CheckPasswordAsync(exsistUser, resetPassword.Password))
            {
                ModelState.AddModelError("", "This password is your last password");
                return View(resetPassword);
            }



            await _userManager.ResetPasswordAsync(exsistUser, resetPassword.Token, resetPassword.Password);

            await _userManager.UpdateSecurityStampAsync(exsistUser);

            return RedirectToAction(nameof(Login));
        }

    
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
