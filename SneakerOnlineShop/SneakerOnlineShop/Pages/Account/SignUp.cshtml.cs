using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Text;

namespace SneakerOnlineShop.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        public SignUpModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Models.Account Account { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var acc = await dBContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email));
                if (acc != null)
                {
                    ViewData["msg"] = "This email already existed";
                    return Page();
                }
                else
                {
                    string cusId = RandomCustomerId(5);
                    var checkCus = await dBContext.Customers.Where(c => c.CustomerId.Equals(cusId)).FirstOrDefaultAsync();
                    while (checkCus != null)
                    {
                        cusId = RandomCustomerId(5);
                    }
                    var cust = new Customer()
                    {
                        CustomerId = cusId,
                        CompanyName = Customer.CompanyName,
                        ContactTitle = Customer.ContactTitle,
                        Address = Customer.Address,
                        ContactName = Customer.ContactName
                    };
                    var newAcc = new Models.Account()
                    {   CreateDate = Account.CreateDate,
                        Email = Account.Email,
                        Password = Account.Password,
                        CustomerId = cust.CustomerId,
                        RoleId = 2
                    };
                    await dBContext.Customers.AddAsync(cust);
                    await dBContext.Accounts.AddAsync(newAcc);
                    await dBContext.SaveChangesAsync();
                    return RedirectToPage("/index");
                }
            } else
            {
                return Page();
            }
        }

        public string RandomCustomerId(int length)
        {
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
            return str_build.ToString();
        }

    }
}
