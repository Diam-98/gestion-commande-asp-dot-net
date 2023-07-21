using Microsoft.AspNetCore.Mvc;
using System;

namespace GestionCommandes.Views.Shared.Components.SearchBar
{
    public class SearchBarViewComponent: ViewComponent
    {
        public SearchBarViewComponent() { }
        public IViewComponentResult Invoke(SPager SearchPager)
        {
            return View("Default", SearchPager);
        }
    }
}
