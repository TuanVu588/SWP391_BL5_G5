using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Text.RegularExpressions;

namespace SneakerOnlineShop.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        [BindProperty]
        public  Models.Account Account { get; set; }
        public ChangePasswordModel(SWP391_DBContext dBContext)
        {
            this._dbContext = dBContext;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(String password,String rePassword)
        {
            if (!IsStrongPassword(password))
            {
                ViewData["invalid"] = "Invalid password. Enter again";
                return Page();
            }

            //check trung voi pass hay khong
            if(!password.Equals(rePassword)) {
                ViewData["notMatch"] = "Password not match repassword";
                return Page();
            }
            //lay ra account co account co email va pass giong
            var acc = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(Account.Password));
            if(acc == null)
            {
                ViewData["PassOrEmailInvalid"] = "Password or Email is not valid";
                return Page();
            }
            //cap nhat lai account
            acc.Password = password;
            _dbContext.SaveChangesAsync();
            return Page();
        }

        private bool IsStrongPassword(string password)
        {
            //123a@123A
            //Has minimum 8 characters in length.Adjust it by modifying { 8,}
            //At least one uppercase English letter. You can remove this condition by removing(?=.*?[A - Z])
            //At least one lowercase English letter.  You can remove this condition by removing(?=.*?[a - z])
            //At least one digit. You can remove this condition by removing(?=.*?[0 - 9])
            //At least one special character,  You can remove this condition by removing(?=.*?[#?!@$%^&*-])
            Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            return regex.IsMatch(password);
        }
    }
}
