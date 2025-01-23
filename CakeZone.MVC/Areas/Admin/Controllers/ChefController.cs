using CakeZone.BL.Extensions;
using CakeZone.BL.ViewModels.Chef;
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
    public class ChefController(AppDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chefs.Include(x => x.Designation).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var data = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();
            if (data.Count == 0 || data is null)
            {
                TempData["ErrorMessage"] = "Ilk once designation yaradin ve ya movcud olan bir designationu show edin.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChefVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();

                return View();
            };

            if (!vm.Image.IsValidType("image"))
            {
                ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();
                ModelState.AddModelError("Image", "File type must be image");
                return View();
            }
            if (!vm.Image.IsValidSize(5 * 1024))
            {
                ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();
                ModelState.AddModelError("Image", "File must be less than 5MB");
                return View();
            }

            string fileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "chef");

            Chef chef = new Chef
            {
                FullName = vm.FullName,
                DesignationId = vm.DesignationId,
                ImageUrl = fileName
            };

            await _context.Chefs.AddAsync(chef);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();

            if (!id.HasValue) return BadRequest();

            var data = await _context.Chefs.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data is null) return NotFound();

            UpdateChefVM vm = new UpdateChefVM
            {
                FullName = data.FullName,
                DesignationId = data.DesignationId,
                ExistImageUrl = data.ImageUrl
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateChefVM vm)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Chefs.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (data is null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();

                vm.ExistImageUrl = data.ImageUrl;
                return View(vm);
            };

            if (vm.Image != null)
            {
                ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();

                if (!ModelState.IsValid)
                {
                    ViewBag.Designations = await _context.Designations.Where(x => !x.IsDeleted).ToListAsync();

                    return View(vm);
                };

                if (!vm.Image.IsValidType("image"))
                {
                    ModelState.AddModelError("Image", "File type must be image");
                    return View(vm);
                }
                if (!vm.Image.IsValidSize(5 * 1024))
                {
                    ModelState.AddModelError("Image", "File must be less than 5MB");
                    return View(vm);
                }

                string oldPath = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "chef", "imgs", data.ImageUrl);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                string newFileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "chef");
                data.ImageUrl = newFileName;
            }

            data.FullName = vm.FullName;
            data.DesignationId = vm.DesignationId;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Chefs.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            string oldPath = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "chef", "imgs", data.ImageUrl);

            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _context.Chefs.Remove(data);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Chefs.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            data.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Chefs.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (data is null) return NotFound();

            data.IsDeleted = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
