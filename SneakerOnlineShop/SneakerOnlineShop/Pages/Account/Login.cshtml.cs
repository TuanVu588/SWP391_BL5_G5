using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyRazorPage.common;
using SneakerOnlineShop.Models;
using System.Security.Principal;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        [BindProperty]
        public Models.Account? account { get; set; }

        public LoginModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public void OnGet()
        {
            HttpContext.Session.Remove("account");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var acc = await dBContext.Accounts.FirstOrDefaultAsync(
                  a => a.Email.Equals(account.Email) && a.Password.Equals(account.Password));
                if (acc is not null)
                {
                    HttpContext.Session.SetString("account", JsonSerializer.Serialize(acc));

                    //read session
                    var session = HttpContext.Session;
                    string key_access = "account";
                    string json = session.GetString(key_access);
                    Console.WriteLine(json);
                    //TODO: return Page
                    if (acc.RoleId == 3)
                    {
                        return RedirectToPage("/admin/product/index");
                    }
                    else
                    {
                        return RedirectToPage("/index");
                    }
                }
                else
                {
                    ViewData["message"] = "This account doesn't exist";
                    return Page();
                }
            }
            return Page();
        }

        public Models.Account findByEmailAndPassword(String email, String password)
        {
            var accountInDB = dBContext.Accounts
                .FirstOrDefault(x => x.Email == email && x.Password == password);
            return accountInDB;
        }
    }
}
