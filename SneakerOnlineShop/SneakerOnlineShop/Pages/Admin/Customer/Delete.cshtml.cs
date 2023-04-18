using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Admin.Customer
{
    public class DeleteModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        public DeleteModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            var customer = await dBContext.Customers.Where(c => c.CustomerId.Equals(id)).FirstOrDefaultAsync();
            if (customer != null)
            {
                dBContext.Customers.Remove(customer);
                dBContext.SaveChanges();
            }
            return RedirectToPage("/admin/customer/index");
        }
    }
}
