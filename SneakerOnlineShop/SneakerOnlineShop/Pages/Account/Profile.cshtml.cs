using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        [BindProperty]
        public Models.Account Account { get; set; }

        public Customer Customer = new Customer();

        public ProfileModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<IActionResult> OnGet(int accId)
        {
            String? accountSession = HttpContext.Session.GetString("account");
            if (accountSession is not null)
            {
                var acc = JsonSerializer.Deserialize<Models.Account>(accountSession);
                if (acc is not null && acc.RoleId == 1)
                {
                    //        if (accId == 0)
                    //{
                    //    return RedirectToPage("/account/login");
                    //}
                    //var acc = await dBContext.Accounts.Where(a => a.AccountId == accId).FirstOrDefaultAsync();
                    //if (acc.RoleId == 3)
                    //{
                    //    var emp = await dBContext.Employees.Where(e => e.EmployeeId == acc.EmployeeId).FirstOrDefaultAsync();
                    //}
                    //if (acc.RoleId == 1)
                    //{
                    var cust = await dBContext.Customers.Where(c => c.CustomerId.Equals(acc.CustomerId)).FirstOrDefaultAsync();
                    //ViewData["CompanyName"] = cust.CompanyName;
                    //ViewData["CompanyTitle"] = cust.ContactTitle;
                    //ViewData["ContactName"] = cust.ContactName;
                    //ViewData["Address"] = cust.Address;
                    if (cust != null)
                    {
                        Customer.CustomerName = cust.CustomerName;
                        Customer.CompanyName = cust.CompanyName;
                        Customer.ContactTitle = cust.ContactTitle;
                        Customer.ContactName = cust.ContactName;
                        Customer.Phone = cust.Phone;
                        Customer.Address = cust.Address;
                    }
                    return Page();
                }
                else
                {
                    return Redirect("/account/login");
                }
            }
            else
            {
                return Redirect("/account/login");
            }
        }
    }
}
