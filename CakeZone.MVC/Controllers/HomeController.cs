using CakeZone.DAL.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeZone.MVC.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chefs.Where(x => !x.IsDeleted).Include(x => x.Designation).ToListAsync());
        }

        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
