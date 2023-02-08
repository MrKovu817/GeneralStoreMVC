using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

    [Route("[controller]")]
    public class TransactionController : Controller
    {

        private readonly GeneralStoreDbContext _ctx;
        public TransactionController(GeneralStoreDbContext ctx)
        {
            _ctx = ctx;
        }
        
        [HttpGet,Route("Transaction")]
        public async Task<IActionResult> Index()
        {
            var GeneralStoreDbContext = _ctx.Transactions.Include(t => t.Customer).Include(t => t.Product).ToListAsync();
            return View(await GeneralStoreDbContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        // GET: Transaction/Details/
        [HttpGet,Route("Transaction/Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _ctx.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transaction/Create
        [HttpGet,Route("Transaction/Create")]
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(_ctx.Customers, "Id", "Email");
            ViewData["ProductId"] = new SelectList(_ctx.Products, "Id", "Name");
            return View();
        }

        // POST: Transaction/Create
        [HttpPost,Route("Transaction/Create/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,CustomerId,Quantity,DateOfTransaction")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _ctx.Add(transaction);
                await _ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_ctx.Customers, "Id", "Email", transaction.CustomerId);
            ViewData["ProductId"] = new SelectList(_ctx.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transaction/Edit/
        [HttpGet,Route("Transaction/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _ctx.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_ctx.Customers, "Id", "Email", transaction.CustomerId);
            ViewData["ProductId"] = new SelectList(_ctx.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // POST: Transaction/Edit/
        [HttpPost,Route("Transaction/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,CustomerId,Quantity,DateOfTransaction")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ctx.Update(transaction);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["CustomerId"] = new SelectList(_ctx.Customers, "Id", "Email", transaction.CustomerId);
            ViewData["ProductId"] = new SelectList(_ctx.Products, "Id", "Name", transaction.ProductId);
            return View(transaction);
        }

        // GET: Transaction/Delete/
        [HttpGet,Route("Transaction/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _ctx.Transactions
                .Include(t => t.Customer)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _ctx.Transactions.FindAsync(id);
            _ctx.Transactions.Remove(transaction);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _ctx.Transactions.Any(e => e.Id == id);
        }
    }
