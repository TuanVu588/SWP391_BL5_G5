using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Data;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Admin.Category
{

    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        public Models.Category Category { get; set; }
        private const int pageSize = 12;
        public Models.Account? account { get; set; }
        public IndexModel(SWP391_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<IActionResult> OnGet(int numPage, int catId, string txtSearch)
        {
            String? accountSession = HttpContext.Session.GetString("account");
            if (accountSession is not null)
            {
                if (account is not null && account.RoleId == 3)
                {
                    account = JsonSerializer.Deserialize<Models.Account>(accountSession);
                    if (numPage <= 0)
                    {
                        numPage = 1;
                    }

                    var categoryPerPage = await dBContext.Categories.OrderByDescending(p => p.CategoryId).ToListAsync();
                    var category = await dBContext.Categories.CountAsync();
                    int totalCategories = category;
                    if (catId > 0)
                    {
                        categoryPerPage = categoryPerPage.Where(p => p.CategoryId == catId).ToList();
                    }

                    if (txtSearch != null && !txtSearch.Equals(""))
                    {
                        try
                        {
                            categoryPerPage = categoryPerPage.Where(p => p.CategoryName.Contains(txtSearch)).ToList();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                    categoryPerPage = categoryPerPage.Skip((numPage - 1) * pageSize).Take(pageSize).ToList();

                    ViewData["productPerPage"] = categoryPerPage;
                    ViewData["maxPage"] = (totalCategories / pageSize) + (totalCategories % pageSize == 0 ? 0 : 1);
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
