using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Roadsters.Data;
using Roadsters.Models.ViewModels;

namespace Roadsters.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShopController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                Bike = await _db.Bikes.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Categories.ToListAsync(),
                Accessorie = await _db.Accessories.ToListAsync(),
                Coupon = await _db.Coupons.Where(c => c.IsActive == true).ToListAsync()

            };
            return View(IndexVM);
        }
    }
}