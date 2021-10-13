using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20210928.Data;
using _20210928.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using _20210928.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace _20210928.Controllers
{
    public class ItemsController : Controller
    {
        private readonly StoreContext _context;
        private readonly SignInManager<StoreUser> _signInManager;

        public ItemsController(StoreContext context, 
            SignInManager<StoreUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: Items
        public async Task<IActionResult> Index(int page = 1, 
            [FromQuery(Name = "sort-by")] string sortBy = "")
        {
            IQueryable<Item> sortedItems = sortBy switch
            {
                "name" => _context.Items.OrderBy(item => item.Name),
                "description" => _context.Items.OrderBy(item => item.Description),
                "price" => _context.Items.OrderBy(item => item.Price),
                _ => _context.Items
            };

            int pageSize = 3;

            int pageCount = 
                (int)Math.Ceiling(_context.Items.Count() / (double)pageSize);

            ViewData["PageCount"] = pageCount;
            ViewData["CurrentPage"] = page;
            ViewData["SortBy"] = sortBy;

            return View(await sortedItems
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.Id = Guid.NewGuid();
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Price")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(Guid id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                Review review = new Review
                {
                    Text = collection["r.Text"],
                    Score = Convert.ToInt32(collection["r.Score"])
                };
                review.Id = Guid.NewGuid();
                review.Item = _context.Items.FirstOrDefault(item => item.Id == id);
                review.UserId = (await _signInManager.UserManager.GetUserAsync(User)).Id;
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }
    }
}
