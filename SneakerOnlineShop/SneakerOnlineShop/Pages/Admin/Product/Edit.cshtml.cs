using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Hubs;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Product
{
    
    public class EditModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        private readonly IHubContext<SignalRServer> context;
        [BindProperty]
        public Models.Product Product { get; set; }

        public EditModel(SWP391_DBContext dBContext, IHubContext<SignalRServer> context)
        {
            this.dBContext = dBContext;
            this.context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var categories = await dBContext.Categories.ToListAsync();
            ViewData["categories"] = categories;

            var product = await dBContext.Products.Where(p => p.ProductId == id).FirstOrDefaultAsync();
            ViewData["ProductId"] = id;
            ViewData["ProductName"] = product.ProductName;
            ViewData["UnitPrice"] = product.UnitPrice;
            ViewData["QuantityPerUnit"] = product.QuantityPerUnit;
            ViewData["UnitsInStock"] = product.UnitsInStock;
            ViewData["CategoryId"] = product.CategoryId;
            ViewData["Color"] = product.Color;
            ViewData["Size"] = product.Size;
            ViewData["Description"] = product.Description;
            /* ViewData["ReorderLevel"] = product.ReorderLevel;*/
            ViewData["UnitsOnOrder"] = product.UnitsOnOrder;
           /* ViewData["Discontinued"] = product.Discontinued;*/

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/
            var product = await dBContext.Products.Where(p => p.ProductId == Product.ProductId).FirstOrDefaultAsync();
            product.ProductName = Product.ProductName;
            product.UnitPrice = Product.UnitPrice;
            product.QuantityPerUnit = Product.QuantityPerUnit;
            product.UnitsInStock = Product.UnitsInStock;
            product.CategoryId = Product.CategoryId;
            product.Size = Product.Size;
            product.Color = Product.Color;
            product.Description =Product.Description;
            product.UnitsOnOrder = Product.UnitsOnOrder;
           /* product.Discontinued = Product.Discontinued;*/

            if(await dBContext.SaveChangesAsync() > 0)
            {
                ViewData["msg"] = "Update successfully";
            }
            return RedirectToPage("/Admin/Product/Index");
        }
    }
}
