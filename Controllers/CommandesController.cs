using GestionCommandes.Data;
using GestionCommandes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GestionCommandes.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System;
using GestionCommandes.Views.Shared.Components.SearchBar;
using Microsoft.Data.SqlClient;

namespace GestionCommandes.Controllers
{
    public class CommandesController : Controller
    {
        private readonly NorthwindContext _context;

        public CommandesController(NorthwindContext context)
        {
            this._context = context;
        }

        public IActionResult Index(string SearchText = "", int pg = 1)
        {
            
            List<Order> orders;
            if(SearchText != "" && SearchText != null)
            {
                orders = _context.Orders
                    .Where(o => o.CustomerId.Contains(SearchText))
                    .ToList();
            }
            else

            orders =   _context.Orders.ToList();
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int recsCount = orders.Count();
            int rekSkip = (pg - 1) * pageSize;
            List<Order> data = orders.Skip(rekSkip).Take(pageSize).ToList();

           SPager SearchPager = new SPager(recsCount,pg,pageSize)
            {
                Action = "Index",
                Controller = "Commandes",
                SearchText = SearchText
            };
           ViewBag.SearchPager = SearchPager;
           return View(data);
        }

        public IActionResult Details(int Id) {
            Order order = _context.Orders.Where(c => c.OrderId == Id).FirstOrDefault();
            return View(order);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", order.EmployeeId);
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName", order.ShipVia);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,EmployeeId,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", order.EmployeeId);
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName", order.ShipVia);
            return View(order);
            }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            OrderDetail od = _context.OrderDetails.Where(c => c.OrderId == Id).FirstOrDefault();
            Order order = _context.Orders.Where(c => c.OrderId == Id).FirstOrDefault();
            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(Order order)
        {
            _context.Attach(order);
            _context.Entry(order).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            OrderCreateModel orderCreateModel = new OrderCreateModel();
            orderCreateModel.order = new Order();
            List<SelectListItem> customers = _context.Customers
                .OrderBy(c => c.CustomerId)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId,
                    Text = c.CustomerId
                }).ToList();
            List<SelectListItem> employees = _context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToString(e.EmployeeId),
                    Text = e.FirstName
                }).ToList();
            List<SelectListItem> shippers = _context.Shippers
                .OrderBy(s => s.CompanyName)
                .Select(s => new SelectListItem
                {
                    Value = Convert.ToString(s.ShipperId),
                    Text = s.CompanyName
                }).ToList();

            List<SelectListItem> products = _context.Products
               .OrderBy(c => c.ProductName)
               .Select(c => new SelectListItem
               {
                   Value = Convert.ToString(c.ProductId),
                   Text = c.ProductName
               }).ToList();

            orderCreateModel.Customers = customers;
            orderCreateModel.Employees = employees;
            orderCreateModel.Shippers = shippers;
           // orderCreateModel.Products = products;
            return View(orderCreateModel);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
           
            _context.Attach(order);
            _context.Entry(order).State = EntityState.Added;
            _context.SaveChanges();
            //SqlCommand cmd = new SqlCommand("select * from Orders where OrderID ="+order.OrderId);
            //cmd.CommandText = "INSERT INTO [Order Details] (OrderID,ProductID) values (@order.OrderId)";
            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult GetProductPrices(int productId)
        {
            if (productId != 0)
            {
                List<SelectListItem> prices = _context.Products
                    .Where(p => p.ProductId == productId)
                    .OrderBy(pr => pr.UnitPrice)
                    .Select(pr => new SelectListItem
                    {
                        Value = Convert.ToString(pr.UnitPrice),
                        Text = Convert.ToString(pr.UnitPrice)
                    }).ToList();
                return Json(prices);
            }
            return null;
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        /*[HttpPost]
        public IActionResult SaveOrderDetail(OrderDetail orderDetail)
        {
            _context.Attach(orderDetail);
            _context.Entry(orderDetail).State = EntityState.Added;
            _context.SaveChanges();
            return View(orderDetail);
        }*/
        public void SaveOrderDetail()
        {
           
            List<SelectListItem> products = _context.Products
              .OrderBy(c => c.ProductName)
              .Select(c => new SelectListItem
              {
                  Value = Convert.ToString(c.ProductId),
                  Text = c.ProductName
                 
              }).ToList();
        }
    }
}
