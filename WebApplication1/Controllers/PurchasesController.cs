using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ProductsContext _context;

        public PurchasesController(ProductsContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index(string includeall)
        {
            
            if (string.IsNullOrEmpty(includeall))
                includeall = "false";
            else
                includeall = (includeall == "true")?"false": "true";

            ViewData["includeCancelledOrders"] = includeall;

            // We need to include only the order that are not cancelled
            var productsContext = _context.Purchase.Include(p => p.Product).Include(p => p.User).Where(p => p.CancelDate == null);
            if (includeall == "true")
                productsContext = _context.Purchase.Include(p => p.Product).Include(p => p.User);
                
            return View(await productsContext.AsNoTracking().ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create(int productId)
        {
            // Some of the fields could be provided with default values. The UserID should come from the JWT token, the product ID should be passed
            Purchase order = new Purchase();
            order.ProductId = productId;
            order.OrderDate = DateTime.Now;
            order.Quantity = 1;
            
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Customer, "Id", "Email");
            return View(order);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ProductId,Quantity,OrderDate,CancelDate")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", purchase.ProductId);
            ViewData["UserId"] = new SelectList(_context.Customer, "Id", "Email", purchase.UserId);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", purchase.ProductId);
            ViewData["UserId"] = new SelectList(_context.Customer, "Id", "Email", purchase.UserId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductId,Quantity,OrderDate,CancelDate")] Purchase purchase)
        {
            if (id != purchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", purchase.ProductId);
            ViewData["UserId"] = new SelectList(_context.Customer, "Id", "Email", purchase.UserId);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            purchase.CancelDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.Id == id);
        }
    }
}
