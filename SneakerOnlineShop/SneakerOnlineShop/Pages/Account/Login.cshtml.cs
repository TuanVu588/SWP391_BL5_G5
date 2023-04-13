using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;

        [BindProperty]
        public Models.Account? Account { get; set; }

        public LoginModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
    }
}
