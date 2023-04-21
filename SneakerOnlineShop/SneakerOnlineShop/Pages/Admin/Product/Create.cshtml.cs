using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SneakerOnlineShop.Hubs;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Product
{
    
    public class CreateModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        private readonly IHubContext<SignalRServer> context;
        [BindProperty]
        public Models.Product Product { get; set; }

        public CreateModel(SWP391_DBContext dBContext, IHubContext<SignalRServer> context)
        {
            this.dBContext = dBContext;
            this.context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var categories = await dBContext.Categories.ToListAsync();
            ViewData["categories"] = categories;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var categories = await dBContext.Categories.ToListAsync();
            ViewData["categories"] = categories;/*
            if (ModelState.IsValid)
            {*/
                
                Models.Product newPro = new Models.Product();
                newPro.ProductName = Product.ProductName;
                newPro.CategoryId = Product.CategoryId;
                newPro.QuantityPerUnit = Product.QuantityPerUnit;
                newPro.UnitPrice = Product.UnitPrice;
                newPro.UnitsInStock = Product.UnitsInStock;
                newPro.UnitsOnOrder = 0;
                newPro.Color = Product.Color;
                newPro.Description = Product.Description;
                newPro.Size = Product.Size;           
                await dBContext.Products.AddAsync(newPro);
                await dBContext.SaveChangesAsync();
                return RedirectToPage("/admin/product/index");
                
            /*}
            return Page();*/
        }
    }
}
