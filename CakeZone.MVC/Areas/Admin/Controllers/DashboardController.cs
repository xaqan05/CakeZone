using CakeZone.CORE.Enums;
using CakeZone.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CakeZone.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DashboardController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
