using GestionCommandes.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionCommandes.ViewModel
{
    public class OrderCreateModel
    {
        public Order order { get; set; }

        public Product product { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> Shippers  { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
    }
}
