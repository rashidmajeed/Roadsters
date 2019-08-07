using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Roadsters.Data;
using Roadsters.Models.ViewModels;
using Roadsters.Models;
using System;
using System.IO;
using Roadsters.Utility;
using System.Linq;

namespace Roadsters.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BikeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }
        public BikeController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            BikeVM = new BikeViewModel()
            {
            Category = _db.Categories,
            Bike = new Bike()
            };
        }
        public async Task<IActionResult> Index()
        {
            var bikes = await _db.Bikes.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return View(bikes);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(BikeVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            BikeVM.Bike.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(BikeVM);
            }

            _db.Bikes.Add(BikeVM.Bike);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var bikeFromDb = await _db.Bikes.FindAsync(BikeVM.Bike.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, BikeVM.Bike.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                bikeFromDb.Image = @"\images\" + BikeVM.Bike.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultBikeImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + BikeVM.Bike.Id + ".png");
                bikeFromDb.Image = @"\images\" + BikeVM.Bike.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BikeVM.Bike = await _db.Bikes.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            BikeVM.SubCategory = await _db.SubCategories.Where(s => s.CategoryId == BikeVM.Bike.CategoryId).ToListAsync();

            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            BikeVM.Bike.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                BikeVM.SubCategory = await _db.SubCategories.Where(s => s.CategoryId == BikeVM.Bike.CategoryId).ToListAsync();
                return View(BikeVM);
            }

            _db.Bikes.Add(BikeVM.Bike);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var bikeFromDb = await _db.Bikes.FindAsync(BikeVM.Bike.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, bikeFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, BikeVM.Bike.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                bikeFromDb.Image = @"\images\" + BikeVM.Bike.Id + extension_new;
            }

            bikeFromDb.Name = BikeVM.Bike.Name;
            bikeFromDb.Description = BikeVM.Bike.Description;
            bikeFromDb.Price = BikeVM.Bike.Price;
            bikeFromDb.Size = BikeVM.Bike.Size;
            bikeFromDb.Stock = BikeVM.Bike.Stock;
            bikeFromDb.CategoryId = BikeVM.Bike.CategoryId;
            bikeFromDb.SubCategoryId = BikeVM.Bike.SubCategoryId;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET : Details Bike
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BikeVM.Bike = await _db.Bikes.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (BikeVM.Bike == null)
            {
                return NotFound();
            }

            return View(BikeVM);
        }

        //GET : Delete Bike
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BikeVM.Bike = await _db.Bikes.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (BikeVM.Bike == null)
            {
                return NotFound();
            }

            return View(BikeVM);
        }

        //POST Delete Bike
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Bike bike = await _db.Bikes.FindAsync(id);

            if (bike != null)
            {
                var imagePath = Path.Combine(webRootPath, bike.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.Bikes.Remove(bike);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }


    }
}