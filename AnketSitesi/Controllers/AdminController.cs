using AnketSitesi.Models;
using AnketSitesi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnketSitesi.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {

        private UserManager<AppUser> _userManager;

        private RoleManager<AppRole> _roleManager;

        private readonly Context _context;

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, Context context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index(AdminPanelIndexViewModel model)
        {
        var anketlistesi=_context.Ankets.ToList();
            var cevaplistesi = _context.CevaplamaDurumus.ToList();
            var kullanıcılistesi = _userManager.Users.ToList();
            var roleistesi = _roleManager.Roles.ToList();

            model = new AdminPanelIndexViewModel
            {
                AnketListesi = anketlistesi,
                CevapListesi = cevaplistesi,
                UserList = kullanıcılistesi
            };



            return View(model);
        }
    }
}
