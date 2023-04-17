using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System;
using System.Collections.Generic;

namespace SneakerOnlineShop.Pages.Employee
{
    public class ViewProductModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        public string PriceSort { get; set; }
        public string SizeFilter { get; set; }
        public string NameFilter { get; set; }
        public string ColorFilter { get; set; }
        public IList<Product> Products { get; set; }

        public ViewProductModel(SWP391_DBContext dBContext)
        {
            this._dbContext = dBContext;
        }
        public async Task OnGet(string PriceSort, string SizeFilter, string NameFilter, String ColorFilter)
        {
            var list = _dbContext.Products.Select(p => p.Size).Distinct().ToList();
            ViewData["listSize"] = list;

            var categories = await _dbContext.Categories.ToListAsync();
            ViewData["categories"] = categories;
            this.PriceSort = PriceSort;
            this.SizeFilter = SizeFilter;
            this.NameFilter = NameFilter;
            this.ColorFilter = ColorFilter;

            IQueryable<Product> productIQ = from s in _dbContext.Products.Include(p => p.ProductImages)
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
