using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Account
{
    public class OrderStatusModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        [BindProperty]
        public Models.Account? account { get; set; }
        IMapper _mapper;

        public List<Order> orders { get; set; }
        public OrderStatusModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> OnGet(String cusid, string fromDate, string toDate)// order cua cusnao
        {

            String? accountSession = HttpContext.Session.GetString("account");
            if (accountSession is not null)
            {
                account = JsonSerializer.Deserialize<Models.Account>(accountSession);
                if (account is not null && account.RoleId == 1)
                {
                    //bussness logic-code cua mn
                    //lay order cua cusdo
                    orders = _dbContext.Orders
                        .Where(o => o.CustomerId.Equals(cusid))
                        .Include(x => x.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductImages)
                        .OrderByDescending(s => s.OrderDate).ToList();
                    //select list order with order details of the customer mang theo customer id theo get?
                    if (fromDate != null && !fromDate.Equals(""))
                    {
                        orders = orders.Where(o => o.OrderDate >= DateTime.Parse(fromDate)).ToList();
                    }
                    if (toDate != null && !toDate.Equals(""))
                    {
                        orders = orders.Where(o => o.OrderDate <= DateTime.Parse(toDate)).ToList();
                    }
                    ViewData["fromDate"] = fromDate;
                    ViewData["toDate"] = toDate;

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
