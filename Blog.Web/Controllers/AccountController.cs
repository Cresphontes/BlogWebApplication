using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.BLL.Services;
using Blog.DAL.Data;
using Blog.Models.Enums;
using Blog.Models.IdentityModels;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Blog.BLL.Helpers.StringHelpers;

namespace Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;

        }



        //Başlangıçta Admin kaydı oluşturmayı sağlayan kod bloğu.(Register)

        //[HttpGet]
        //public async Task<IActionResult> Register()
        //{
        //    ApplicationUser user = new ApplicationUser();

        //    user.UserName = "admin";
        //    string password = "Admin123*";
        //    user.BirthDate = new DateTime(1994, 10, 15);
        //    user.Name = "Aykut";
        //    user.Surname = "KARAGOZ";
        //    user.Email = "aykutkaragoz94@gmail.com";
        //    user.PhoneNumber = "05468737634";

        //    var result = await _userManager.CreateAsync(user, password);

        //    if (result.Succeeded)
        //    {
        //        await CreateRoles();
        //        await _userManager.AddToRoleAsync(user, IdentityRoles.Admin.ToString());
        //    }

        //    return View("Login");
        //}

        //private async Task CreateRoles()
        //{
        //    var roleNames = Enum.GetNames(typeof(IdentityRoles));

        //    foreach (var roleName in roleNames)
        //    {
        //        if (!_roleManager.RoleExistsAsync(roleName).Result)
        //        {
        //            await _roleManager.CreateAsync(new ApplicationRole()
        //            {
        //                Name = roleName
        //            });
        //        }
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.UserName == null || model.Password==null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre alanlarını doldurunuz.");
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe,
                false);

            if (result.Succeeded)
            {

                return RedirectToAction("AdminPanel", "Admin");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return View("Login");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                EditProfileViewModel model = new EditProfileViewModel()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Surname = user.Surname,
                    PhoneNumber = user.PhoneNumber
                };

                return View(model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errMsg = "";

                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        errMsg += error.ErrorMessage + "\n";
                    }
                }

                return Json(new ResponseData()
                {
                    Success = false,
                    Message = errMsg,
                });
            }

            var user = await _userManager.GetUserAsync(User);

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return Json(new ResponseData()
            {
                Success = true,
                Message = "Profil bilgileriniz başarı ile değiştirilmiştir."
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> EditPassword(EditPasswordViewModel model)
        {
      

            var errMsg = "";

            if (!ModelState.IsValid)
            {
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        errMsg += error.ErrorMessage + "\n";
                    }
                }
                return Json(new ResponseData()
                {
                    Success = false,
                    Message = "Bir hata oluştu, " + errMsg
                });
            }


            var user = await _userManager.GetUserAsync(User);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmNewPassword);


            if (result.Succeeded)
            {
                return Json(new ResponseData()
                {
                    Success = true,
                    Message = "Şifre değiştirme işlemi başarılı"
                });
            }
            else
            {
                foreach (var identityError in result.Errors)
                {
                    errMsg += identityError.Description + "\n";
                }
                return Json(new ResponseData()
                {
                    Success = false,
                    Message = "Bir hata oluştu, " + errMsg,
                });
            }
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var users = _userManager.Users;
            ApplicationUser _user = null;

            foreach (var user in users)
            {
                if (user.Email == model.Email)
                {
                    _user = user;
                }
                else
                {
                    TempData["Message"] = "Girdiğiniz Email Adresine Ait Kayıtlı Bir Kullanıcı Bulunamadı.";
                    return View();
                }
            }


            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(_user);
           
            string tempPass = (GetCode().Substring(0, 8));
            var lowerLetter = tempPass.Substring(0, 1).ToUpper();

            Random rnd = new Random();
            var number = rnd.Next(1, 10);

            string newPass = lowerLetter + tempPass + number+".";

            var result = await _userManager.ResetPasswordAsync(_user, resetToken, newPass);

            if (result.Succeeded)
            {
                string siteUrl = HttpContext.Request.GetEncodedUrl().Substring(0, 23);

                var emailService = new EmailService();

                var body = $"Merhaba <b>{_user.Name} {_user.Surname}</b><br> Yeni Şifreniz : {newPass} <br>Aşağıdaki linke tıklayarak hesabınıza yeni şifreniz ile giriş yapabilirsiniz <br><a href='{siteUrl}/Account/Login'>Giriş Yap</a>";

                await emailService.SendAsync(new MessageViewModel()
                {
                    Body = body,
                    Subject = "Şifremi Unuttum."

                }, _user.Email);



                TempData["Message"] = "Şifre Sıfırlama Maili Email Adresinize Gönderildi.";
                return View();
            }
            else
            {
                TempData["Message"] = "Bir Hata Oluştu Lütfen Tekrar Deneyiniz.";
                return View();
            }

        }

    }

}


