using AnketSitesi.Models;
using AnketSitesi.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;

namespace AnketSitesi.Controllers
{
  
    public class AccountController: Controller
    {
      
        private UserManager<AppUser> _userManager;

        private RoleManager<AppRole> _roleManager;

        private SignInManager<AppUser> _signInManager;

        private IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    if (!await _userManager.IsEmailConfirmedAsync(user)) 
                    {
                        ModelState.AddModelError("", "Hesabınızı Onaylayınız");
                        return View(model);
                    }
                    
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        return RedirectToAction("Index2", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutdate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeleft = lockoutdate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız kitlendi, lütfen {timeleft.Minutes} dakika sonra deneyiniz.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Hatalı parola");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Hatalı e-mail yada parola");
                }
            }
            return View(model);
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName };

                // Şifreyi ekleyin
                var result = await _userManager.CreateAsync(user, model.Password);



                if (result.Succeeded)
                {

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action("ConfirmEmail", "Account", new { user.Id, token });
                    MimeMessage mimeMessage = new MimeMessage();
                    MailboxAddress mailboxAddressfrom = new MailboxAddress("Pollster", "h.kerem107061@gmail.com");
                    MailboxAddress mailboxAddressto = new MailboxAddress("User", user.Email);
                    mimeMessage.From.Add(mailboxAddressfrom);
                    mimeMessage.To.Add(mailboxAddressto);

                    var bodybuilder=new BodyBuilder();
                    bodybuilder.TextBody = $"Onaylama için linke tıklayın'http://localhost:5261{url}'";
                    mimeMessage.Body=bodybuilder.ToMessageBody();

                    mimeMessage.Subject = "Email Onaylama";

                    SmtpClient smtpClient = new SmtpClient();

                    smtpClient.Connect("smtp.gmail.com", 587, false);
                    smtpClient.Authenticate("h.kerem107061@gmail.com", "iygbgystppchtlyk");

                    smtpClient.Send(mimeMessage);
                    smtpClient.Disconnect(true);



                 
                    return RedirectToAction("Login","Account");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ConfirmEmail(string Id,string token)
        {

            var user=await _userManager.FindByIdAsync(Id);

            if (user != null) 
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                
                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız onaylandı";
                    return RedirectToAction("ConfirmEmail", "Account");
                }
            
            
            }
            TempData["message"] = "Kullanıcı bulunamadı";
            return View();

        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData["message"] = "Eposta adresinizi giriniz";
                return View();  
            }
          var user=await _userManager.FindByEmailAsync(Email);
            if (user==null)
            {
                TempData["message"] = "Kayıtlı eposta adresi bulunamadı";
                return View();

            }
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { user.Id, token });
            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddressfrom = new MailboxAddress("Pollster", "h.kerem107061@gmail.com");
            MailboxAddress mailboxAddressto = new MailboxAddress("User", user.Email);
            mimeMessage.From.Add(mailboxAddressfrom);
            mimeMessage.To.Add(mailboxAddressto);

            var bodybuilder = new BodyBuilder();
            bodybuilder.TextBody = $"Şifreyi Sıfırlama için linke tıklayın'http://localhost:5261{url}'";
            mimeMessage.Body = bodybuilder.ToMessageBody();

            mimeMessage.Subject = "Şifre Sıfılama";

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("h.kerem107061@gmail.com", "iygbgystppchtlyk");

            smtpClient.Send(mimeMessage);
            smtpClient.Disconnect(true);


            TempData["message"] = "Eposta adresinize gönderilen e-postayı link ile şifrenizi sıfırlayabilirsiniz";
            return View();


        }


        public IActionResult ResetPassword(string Id,string token)
        {
            if (Id==null || token==null)
            {
                return RedirectToAction("Login");
            }
            var model =new ResetPasswordModel {Token=token };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user==null)
                {
                    TempData["message"] = "Bu mail adresi ile eşleşen kullanıcı yok";
                    return RedirectToAction("Login");
                }

                var result=await _userManager.ResetPasswordAsync(user, model.Token,model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = "Parolanız sıfırlandı";
                    return RedirectToAction("Login");
                }


                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }



            }
            return View(model);

        }


        [HttpGet] 
        public async Task<IActionResult> AccountEdit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(id);

         

                return View(new AccountEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
               
                });


            

           
        }

        [HttpPost]
        public async Task<IActionResult> AccountEdit(string id, AccountEditViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }



            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.FullName = model.FullName;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }

             
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }

                }
            }

            return View(model );
        }

      


    }

}

