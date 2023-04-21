using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Category
{
    
    public class CreateModel : PageModel
    {       
        private readonly SWP391_DBContext dBContext;
        [BindProperty]
        public Models.Category Category { get; set; }
        
        public CreateModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnPost()
        {
            
            Models.Category newCat = new Models.Category();
            newCat.CategoryName = Category.CategoryName;
            await dBContext.Categories.AddAsync(newCat);
            await dBContext.SaveChangesAsync();
            return RedirectToPage("/admin/category/index");
        }
    }
}
