using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Category
{
    
    public class UpdateModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        [BindProperty]
        public Models.Category Category { get; set; }

        public UpdateModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var category = await dBContext.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();
            ViewData["CategoryId"] = category.CategoryId;
            ViewData["CategoryName"] = category.CategoryName;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var category = await dBContext.Categories.Where(c => c.CategoryId == Category.CategoryId).FirstOrDefaultAsync();
            category.CategoryName = Category.CategoryName;
            if(await dBContext.SaveChangesAsync() > 0)
            {
                ViewData["msg"] = "Update successfully";
            }
            return RedirectToPage("/admin/category/Index");
        }
    }
}
