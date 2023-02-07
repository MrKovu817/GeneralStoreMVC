using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GSMVC.Data;
using GSMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

{
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        private readonly GeneralStoreDbContext _ctx;
        public CustomerController(GeneralStoreDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IActionResult> Index()
        {
            var customers = _ctx.Customers.Select(customer => new CustomerIndexModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            });
            return View(customers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMsg"] = "Model State is Invalid";
                return View(model);
            }
            _ctx.Customers.Add(new Customer
            {
                Name = model.Name,
                Email = model.Email
            });
            if (_ctx.SaveChanges() == 1)
            {
                return Redirect("/Customer");
            }
            TempData["ErrorMsg"] = "Unable to save to the database. Please try again later.";
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = _ctx.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = new CustomerDetailModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }             
            var customer = _ctx.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = new CustomerEditModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CustomerEditModel model)
        {
            var customer = _ctx.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer.Name = model.Name;
            customer.Email = model.Email;

            if (_ctx.SaveChanges() == 1)
            {
                return Redirect("/customer");
            }

            ViewData["ErrorMsg"] = "Unable to save to the database. Please try again later.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var customer = _ctx.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            var model = new CustomerDetailModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, CustomerDetailModel model)
        {
            var customer = _ctx.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            _ctx.Customers.Remove(customer);
            _ctx.SaveChanges();
            return Redirect("/Customer");
        }
    }
}