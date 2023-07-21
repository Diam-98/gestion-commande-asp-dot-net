using GestionCommandes.Data;
using GestionCommandes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GestionCommandes.Controllers
{
    public class DetailCommandesController : Controller
    {
        private readonly NorthwindContext _context;

        public DetailCommandesController(NorthwindContext context)
        {
           this._context = context;
        }

        public IActionResult Index()
        {
            List<OrderDetail> orders = _context.OrderDetails.ToList();
            return View(orders);
        }
    }
}
