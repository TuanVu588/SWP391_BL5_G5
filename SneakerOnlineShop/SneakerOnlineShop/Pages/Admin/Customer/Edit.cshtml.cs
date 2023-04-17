using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Admin.Customer
{
    public class EditModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        [BindProperty]
        public Models.Customer Customer { get; set; }

        public EditModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            var customer = await dBContext.Customers.Where(c => c.CustomerId.Equals(id)).FirstOrDefaultAsync();
            ViewData["CustomerID"] = customer.CustomerId;
            ViewData["CustomerName"] = customer.CustomerName;
            ViewData["CompanyName"] = customer.CompanyName;
            ViewData["Phone"] = customer.Phone;
            ViewData["Address"] = customer.Address;
            ViewData["ContactName"] = customer.ContactName;
            ViewData["ContactTitle"] = customer.ContactTitle;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var customer = await dBContext.Customers.Where(c => c.CustomerId.Equals(Customer.CustomerId)).FirstOrDefaultAsync();
            customer.CustomerName = Customer.CustomerName;
            customer.CompanyName = Customer.CompanyName;
            customer.Phone = Customer.Phone;
            customer.Address = Customer.Address;
            customer.ContactName = Customer.ContactName;
            customer.ContactTitle = Customer.ContactTitle;
            if(await dBContext.SaveChangesAsync() > 0)
            {
                ViewData["msg"] = "Update successfully";
            }
            return RedirectToPage("/admin/customer/index");
        }
    }
}
