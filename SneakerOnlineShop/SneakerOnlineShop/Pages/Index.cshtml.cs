using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        private const int pageSize = 12;

        public IndexModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<IActionResult> OnGet(int numPage)

        {
            if (numPage <= 0)
            {
                numPage = 1;
            }
            List<Models.Product> proList = dBContext.Products.ToList();
            List<Models.Category> cateList = dBContext.Categories.ToList();
            List<Models.ProductImage> images = dBContext.ProductImages.ToList();
            ViewData["products"] = proList;
            ViewData["catergories"] = cateList;
            ViewData["ProductImages"] = images;
            int total = proList.Count;

            var proListpage = proList.Skip((numPage - 1) * pageSize).Take(pageSize).ToList();

            ViewData["productPerPage"] = proListpage;
            ViewData["maxPage"] = (proList.Count / pageSize) + (proList.Count % pageSize == 0 ? 0 : 1);
            ViewData["page"] = numPage;

            return Page();
        }
    }
}