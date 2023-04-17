using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Account
{
    public class ProfileEditModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        [BindProperty]

        public Customer Customer { get; set; }

        [BindProperty]

        public Models.Account Account { get; set; }

        public ProfileEditModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int accId)
        {
            if (accId == 0)
            {
                return RedirectToPage("/index");
            }
            var acc = await dBContext.Accounts.Where(a => a.AccountId == accId).FirstOrDefaultAsync();
            //if (acc.RoleId == 2)
            //{
            //    var emp = await dBContext.Employees.Where(e => e.EmployeeId == acc.EmployeeId).FirstOrDefaultAsync();
            //}
            if (acc.RoleId == 1)
            {
                var cust = await dBContext.Customers.Where(c => c.CustomerId.Equals(acc.CustomerId)).FirstOrDefaultAsync();
                ViewData["CustomerName"] = cust.CustomerName;
                ViewData["CompanyName"] = cust.CompanyName;
                ViewData["CompanyTitle"] = cust.ContactTitle;
                ViewData["ContactName"] = cust.ContactName;
                ViewData["Address"] = cust.Address;
                ViewData["Phone"] = cust.Phone;

                Customer = cust;

            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (ModelState.IsValid)
            {
                string accIdStr = Request.Form["accId"];
                if (accIdStr != null && !accIdStr.Equals(""))
                {
                    Console.WriteLine("Company Name: " + accIdStr);

                    int accId = int.Parse(accIdStr);
                    if (accId == 0)
                    {
                        return RedirectToPage("/index");
                    }
                    var acc = await dBContext.Accounts.Where(a => a.AccountId == accId).FirstOrDefaultAsync();
                    //if (acc.RoleId == 2)
                    //{
                    //    var emp = await dBContext.Employees.Where(e => e.EmployeeId == acc.EmployeeId).FirstOrDefaultAsync();
                    //}
                    if (acc.RoleId == 1)
                    {
                        var cust = await dBContext.Customers.Where(c => c.CustomerId.Equals(acc.CustomerId)).FirstOrDefaultAsync();
                            cust.CustomerName = Customer.CustomerName;
                            cust.CompanyName = Customer.CompanyName;
                            cust.ContactName = Customer.ContactName;
                            cust.ContactTitle = Customer.ContactTitle;
                            cust.Address = Customer.Address;
                            cust.Phone = Customer.Phone;
                            Console.WriteLine("Company Name: " + cust.CompanyName);
                            ViewData["CompanyName"] = cust.CompanyName;
                            ViewData["CompanyTitle"] = cust.ContactTitle;
                            ViewData["ContactName"] = cust.ContactName;
                            ViewData["Address"] = cust.Address;
                            ViewData["Phone"] = cust.Phone;
                            if (await dBContext.SaveChangesAsync() > 0)
                            {
                                ViewData["msg"] = "Update successfully!";
                            }
                        }
                        
                }
                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}