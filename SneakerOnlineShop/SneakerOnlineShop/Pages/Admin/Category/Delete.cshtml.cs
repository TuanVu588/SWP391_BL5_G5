using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;

namespace SneakerOnlineShop.Pages.Admin.Category
{
    
    public class DeleteModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        public DeleteModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var category = await dBContext.Categories.Where(c => c.CategoryId== id).FirstOrDefaultAsync();
            if(category != null)
            {
                dBContext.Categories.Remove(category);
                dBContext.SaveChanges();
            }
            return RedirectToPage("/admin/category/index");
        }
    }
}
