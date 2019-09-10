using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Roadsters.Data;
using Roadsters.Models;
using Roadsters.Models.ViewModels;
using System;
using System.IO;
using Roadsters.Utility;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Roadsters.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccessorieController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;


        [BindProperty]
        public AccessorieViewModel AccessorieVM { get; set; }

        public AccessorieController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            AccessorieVM = new AccessorieViewModel()
            {
                Accessorie = new Accessorie()
            };

        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Accessories.ToListAsync());

        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(AccessorieVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
          if (!ModelState.IsValid)
            {
                return View(AccessorieVM);

            }
            _db.Accessories.Add(AccessorieVM.Accessorie);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var accessorieFromDb = await _db.Accessories.FindAsync(AccessorieVM.Accessorie.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, AccessorieVM.Accessorie.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                accessorieFromDb.Image = @"\images\" + AccessorieVM.Accessorie.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultAccessorieImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + AccessorieVM.Accessorie.Id + ".png");
                accessorieFromDb.Image = @"\images\" + AccessorieVM.Accessorie.Id + ".png";
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

            AccessorieVM.Accessorie = await _db.Accessories.SingleOrDefaultAsync(m => m.Id == id);

            if (AccessorieVM.Accessorie == null)
            {
                return NotFound();
            }
            return View(AccessorieVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var acessorieFromDb = await _db.Accessories.FindAsync(AccessorieVM.Accessorie.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file
                var imagePath = Path.Combine(webRootPath, acessorieFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                //we will upload the new file
                using (var filesStream = new FileStream(Path.Combine(uploads, AccessorieVM.Accessorie.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                acessorieFromDb.Image = @"\images\" + AccessorieVM.Accessorie.Id + extension_new;
            }

            acessorieFromDb.Name = AccessorieVM.Accessorie.Name;
            acessorieFromDb.Description = AccessorieVM.Accessorie.Description;
            acessorieFromDb.Price = AccessorieVM.Accessorie.Price;
            acessorieFromDb.Stock = AccessorieVM.Accessorie.Stock;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //GET : Accessorie Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccessorieVM.Accessorie = await _db.Accessories.SingleOrDefaultAsync(m => m.Id == id);

            if (AccessorieVM.Accessorie == null)
            {
                return NotFound();
            }

            return View(AccessorieVM);
        }

        //GET : Delete Accessorie
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccessorieVM.Accessorie = await _db.Accessories.SingleOrDefaultAsync(m => m.Id == id);

            if (AccessorieVM.Accessorie == null)
            {
                return NotFound();
            }

            return View(AccessorieVM);
        }

        //POST Delete Accessorie
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Accessorie accessorie = await _db.Accessories.FindAsync(id);

            if (accessorie != null)
            {
                var imagePath = Path.Combine(webRootPath, accessorie.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _db.Accessories.Remove(accessorie);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }



    }
}