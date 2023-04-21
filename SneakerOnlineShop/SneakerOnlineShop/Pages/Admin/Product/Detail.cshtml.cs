using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Admin.Product
{
   
    public class DetailModel : PageModel
    {
		private readonly SWP391_DBContext dBContext;

		public DetailModel(SWP391_DBContext dBContext)
		{
			this.dBContext = dBContext;
		}

		public async Task<IActionResult> OnGet(int id)
        {
		/*	if (HttpContext.Session.GetString("CustSession") == null)
			{
				return RedirectToPage("/error");
			}
			else
			{
				var account = JsonSerializer.Deserialize<MyRazorPages.Models.Account>(HttpContext.Session.GetString("CustSession"));
				if (account.Role == 2)
				{
					return RedirectToPage("/error");
				}
			}*/

			var product = await dBContext.Products.SingleOrDefaultAsync(p => p.ProductId == id);
			if (product == null)
			{
				return RedirectToPage("/admin/error404");
			}

			ViewData["product"] = product;

			return Page();
        }
    }
}
