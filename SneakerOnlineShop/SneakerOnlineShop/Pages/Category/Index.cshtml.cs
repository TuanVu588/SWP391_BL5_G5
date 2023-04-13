using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        private const int pageSize = 12;


        public IndexModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int id, int numPage, string sortOrder)
        {
            if (numPage <= 0)
            {
                numPage = 1;
            }
            var categories = await dBContext.Categories.ToListAsync();
            ViewData["categories"] = categories;

            var productsByCateId = await dBContext.Products.Where(p => p.CategoryId == id).CountAsync();
            ViewData["productsByCateId"] = productsByCateId;

            int totalProduct = productsByCateId;
            var productPerPage = await dBContext.Products.Where(p => p.CategoryId == id).ToListAsync();

            switch (sortOrder)
            {
                case "priceAsc":
                    productPerPage = productPerPage.OrderBy(p => p.UnitPrice).ToList();
                    break;
                case "priceDesc":
                    productPerPage = productPerPage.OrderByDescending(p => p.UnitPrice).ToList();
                    break;
                default:
                    break;
            }

            productPerPage = productPerPage.Skip((numPage - 1) * pageSize).Take(pageSize).ToList();

            ViewData["productPerPage"] = productPerPage;
            ViewData["maxPage"] = (totalProduct / pageSize) + (totalProduct % pageSize == 0 ? 0 : 1);
            ViewData["page"] = numPage;
            ViewData["CatId"] = id;
            ViewData["sortOrder"] = sortOrder;
            return Page();
        }
    }
}
