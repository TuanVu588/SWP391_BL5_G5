using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Admin.Customer
{
    
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        public Models.Customer Customer { get; set; }
        private const int pageSize = 12;
        [BindProperty]
        public Models.Account? account { get; set; }
        public IndexModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int numPage, string catId, string txtSearch)
        {
            String? accountSession = HttpContext.Session.GetString("account");

            if(accountSession is not null)
            {
                if(account is not null && account.RoleId == 3)
                {
                    account = JsonSerializer.Deserialize<Models.Account>(accountSession);
                    if (numPage <= 0)
                    {
                        numPage = 1;
                    }
                    var customerPerPage = await dBContext.Customers.ToListAsync();
                    var customers = await dBContext.Customers.CountAsync();
                    int totalCustomers = customers;
                    if (catId != null)
                    {
                        customerPerPage = customerPerPage.Where(p => p.CustomerId.Equals(catId)).ToList();
                    }

                    if (txtSearch != null && !txtSearch.Equals(""))
                    {
                        try
                        {
                            customerPerPage = customerPerPage.Where(p => p.CustomerName.Contains(txtSearch)).ToList();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                    customerPerPage = customerPerPage.Skip((numPage - 1) * pageSize).Take(pageSize).ToList();

                    ViewData["customerPerPage"] = customerPerPage;
                    ViewData["maxPage"] = (totalCustomers / pageSize) + (totalCustomers % pageSize == 0 ? 0 : 1);
                    ViewData["page"] = numPage;
                    ViewData["catId"] = catId;
                    ViewData["txtSearch"] = txtSearch;
                    return Page();
                }
                else
                {
                    return RedirectToPage("/account/login");
                }
            }
            else
            {
                return RedirectToPage("/account/login");
            }
        }
    }
}
