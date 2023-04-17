using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        //private const int pageSize = 12;
        public int cid { get; set; }
        public string PriceSort { get; set; }
        public string SizeFilter { get; set; }
        public string NameFilter { get; set; }
        public string ColorFilter { get; set; }
        public IList<Product> Products { get; set; }

        public IndexModel(SWP391_DBContext dBContext)
        {
            this._dbContext = dBContext;
        }
        public async Task OnGet(int cid, string PriceSort, string SizeFilter, string NameFilter, String ColorFilter)
        {
            //var productsByCateId = await dBContext.Products.Where(p => p.CategoryId == cid).CountAsync();
            //ViewData["productsByCateId"] = productsByCateId;

            //int totalProduct = productsByCateId;
            var productPerPage = await _dbContext.Products.Where(p => p.CategoryId == cid).ToListAsync();

            //my code
            var list = _dbContext.Products.Select(p => p.Size).Distinct().ToList();
            ViewData["listSize"] = list;

            var categories = await _dbContext.Categories.ToListAsync();
            ViewData["categories"] = categories;
            this.PriceSort = PriceSort;
            this.SizeFilter = SizeFilter;
            this.NameFilter = NameFilter;
            this.ColorFilter = ColorFilter;
            this.cid = cid;
            IQueryable<Product> productIQ = from s in _dbContext.Products
                                            .Where(p => p.CategoryId == cid)
                                            .Include(p => p.ProductImages)
                                            select s;
            if (!string.IsNullOrEmpty(SizeFilter))
            {
                productIQ = productIQ.Where(s => s.Size == Int32.Parse(SizeFilter));
            }
            if (!string.IsNullOrEmpty(NameFilter))
            {
                productIQ = productIQ.Where(s => s.ProductName.Contains(NameFilter));
            }
            if (!string.IsNullOrEmpty(ColorFilter))
            {
                productIQ = productIQ.Where(s => s.Color.Contains(ColorFilter));
            }
            switch (PriceSort)
            {
                case "price_desc":
                    productIQ = productIQ.OrderByDescending(s => s.UnitPrice);
                    break;
                case "price_asc":
                    productIQ = productIQ.OrderBy(s => s.UnitPrice);
                    break;
                default:
                    productIQ = productIQ.OrderBy(s => s.UnitPrice);
                    break;
            }
            Products = await productIQ.AsNoTracking().ToListAsync();
        }
    }
}
