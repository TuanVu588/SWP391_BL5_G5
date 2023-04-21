using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Product
{
    
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        public Models.Product Product { get; set; }
        private const int pageSize = 12;

        public IndexModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<IActionResult> OnGet(int numPage, int catId, string txtSearch)
        {
            if (numPage <= 0)
            {
                numPage = 1;
            }
            var categories = await dBContext.Categories.ToListAsync();
            ViewData["categories"] = categories;
            var products = await dBContext.Products.CountAsync();

            int totalProduct = products;
            var productPerPage = await dBContext.Products.OrderByDescending(p => p.ProductId).ToListAsync();
            
            if (catId > 0)
            {
                productPerPage = productPerPage.Where(p => p.CategoryId == catId).ToList();
            }

            if (txtSearch != null && !txtSearch.Equals(""))
            {
                try
                {
                    productPerPage = productPerPage.Where(p => p.ProductId.ToString().Contains(txtSearch)
                                                    || p.ProductName.Contains(txtSearch)
                                                    || p.UnitPrice.ToString().Contains(txtSearch)
                                                    || p.QuantityPerUnit.Contains(txtSearch)
                                                    || p.UnitsInStock.ToString().Contains(txtSearch)
                                                    || p.Category.CategoryName.Contains(txtSearch)
                                                    )
                                                    .ToList();
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            productPerPage = productPerPage.Skip((numPage - 1) * pageSize).Take(pageSize).ToList();

            ViewData["productPerPage"] = productPerPage;
            ViewData["maxPage"] = (totalProduct / pageSize) + (totalProduct % pageSize == 0 ? 0 : 1);
            ViewData["page"] = numPage;
            ViewData["catId"] = catId;
            ViewData["txtSearch"] = txtSearch;
            return Page();
        }

    }
}
