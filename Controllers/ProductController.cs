using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GSMVC.Data;
using GSMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        private readonly GeneralStoreDbContext _ctx;

        public ProductController(GeneralStoreDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _ctx
            .Products
            .Select(p => new ProductIndexModel
            {
                Id = p.Id,
                Name = p.Name,
                QuantityInStock = p.QuantityInStock,
                Price = p.Price,
            })
            .ToListAsync();
            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,QuantityInStock,Price")] ProductCreateModel model)
        {
        if (ModelState.IsValid)
        {
            _ctx.Add(new Product 
        {
            Name = model.Name,
            Price = model.Price,
            QuantityInStock= model.QuantityInStock,
        });
        await _ctx.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        }
        return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _ctx.Products
                .Select(p => new ProductDetailModel 
                {
                    Id = p.Id,
                    Name= p.Name,
                    QuantityInStock = p.QuantityInStock,
                    Price = p.Price
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx
                .Products
                .Select(p => new ProductEditModel 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    QuantityInStock = p.QuantityInStock
                })
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,QuantityInStock,Price")] ProductEditModel model)
        {
            var product = await _ctx.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                product.Name = model.Name;
                product.Price = model.Price;
                product.QuantityInStock = model.QuantityInStock;
                try
                {
                    _ctx.Update(product);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        throw;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _ctx
                .Products
                .Select(p => new ProductDetailModel 
                {
                    Id = p.Id,
                    Price = p.Price,
                    Name = p.Name,
                    QuantityInStock = p.QuantityInStock,
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _ctx.Products.FindAsync(id);
            _ctx.Products.Remove(product);
            await _ctx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}