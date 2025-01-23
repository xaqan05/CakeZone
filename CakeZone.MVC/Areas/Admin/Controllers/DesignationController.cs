using CakeZone.BL.ViewModels.Designation;
using CakeZone.CORE.Enums;
using CakeZone.CORE.Models;
using CakeZone.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeZone.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DesignationController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Designations.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDesignationVM vm)
        {

            if (!ModelState.IsValid) return View();

            if (_context.Designations.Any(x => x.Name.ToLower() == vm.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This designation name is exist");
                return View();
            }

            Designation designation = new Designation
            {
                Name = vm.Name
            };

            await _context.Designations.AddAsync(designation);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Designations.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            UpdateDesignationVM vm = new UpdateDesignationVM
            {
                Name = data.Name
            };


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateDesignationVM vm)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Designations.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            if (_context.Designations.Any(x => x.Name.ToLower() == vm.Name.ToLower()) && vm.Name != data.Name)
            {
                ModelState.AddModelError("Name", "This designation name is exist");
                return View(vm);
            }

            data.Name = vm.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Designations.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            var chefs = await _context.Chefs.ToListAsync();

            if (chefs != null || chefs.Count != 0)
            {
                foreach (var item in chefs)
                {
                    if (item.DesignationId == id)
                    {
                        TempData["ErrorMessage"] = "Bu designationu sile bilmezsiniz.Bu designationa aid olan chef-ler movcuddur";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            _context.Designations.Remove(data);

            await _context.SaveChangesAsync();
            TempData["SuccesMessage"] = "Ugurla silindi";


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Designations.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            var chefs = await _context.Chefs.ToListAsync();

            if (chefs != null || chefs.Count != 0)
            {
                foreach (var item in chefs)
                {
                    if (item.DesignationId == id)
                    {
                        item.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            data.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Designations.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            var chefs = await _context.Chefs.ToListAsync();

            if (chefs != null || chefs.Count != 0)
            {
                foreach (var item in chefs)
                {
                    if (item.DesignationId == id)
                    {
                        item.IsDeleted = false;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            data.IsDeleted = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
