using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SneakerOnlineShop.Models;
using System;
using System.Text;

namespace SneakerOnlineShop.Pages.Admin.Customer
{
    public class CreateModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        [BindProperty]
        public Models.Customer Customer { get; set; }

        public CreateModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnPost()
        {
            int length = 5;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            Models.Customer newCus = new Models.Customer();
            newCus.CustomerId = str_build.ToString();
            newCus.CompanyName = Customer.CompanyName;
            newCus.CustomerName = Customer.CustomerName;
            newCus.Address = Customer.Address;
            newCus.Phone = Customer.Phone;
            newCus.ContactName= Customer.ContactName;
            newCus.ContactTitle = Customer.ContactTitle;
            await dBContext.Customers.AddAsync(newCus);
            await dBContext.SaveChangesAsync();
            return RedirectToPage("/admin/customer/index");
        }
        
    }
}
