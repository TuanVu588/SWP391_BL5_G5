using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyRazorPage.common;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        [BindProperty]
        public Models.Account? account { get; set; }
        [BindProperty]
        public HashSet<int> Years { get; set; }
        public int TotalOrdersNotApproved { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalGuests { get; set; }
        public int TotalGuestInMonth { get; set; }
        public int TotalCustomerInMonth { get; set; }
        public long jan { get; set; }
        public long feb { get; set; }
        public long mar { get; set; }
        public long apr { get; set; }
        public long may { get; set; }
        public long jun { get; set; }
        public long jul { get; set; }
        public long aug { get; set; }
        public long sep { get; set; }
        public long oct { get; set; }
        public long nov { get; set; }
        public long dec { get; set; }

        private readonly SneakerOnlineShop.Models.SWP391_DBContext dBContext;
        private readonly CommonRole commonRole = new();

        public DashboardModel(SneakerOnlineShop.Models.SWP391_DBContext _context)
        {
            this.dBContext = _context;

        }
        public async Task<IActionResult> OnGet(int? Year)
        {
            String? accountSession = HttpContext.Session.GetString("account");
            if (accountSession is not null)
            {

                if (Year is null) Year = DateTime.Now.Year;

                Years = dBContext.Orders.Select(x => x.OrderDate.Value.Year).ToHashSet();
                account = JsonSerializer.Deserialize<Models.Account>(accountSession);
                if (account is not null && account.RoleId == 3)
                {
                    //TotalOrders has not approved
                    TotalOrdersNotApproved = dBContext.Orders.Where(o => o.Status.Equals("Pending")).Count();
                    var TotalOrdersNotApprovedObj = dBContext.Orders.Where(o => o.Status.Equals("Pending")).ToList();
                    //TotalOrders
                    TotalOrders = dBContext.Orders.Count();
                    var TotalOrdersObj = dBContext.Orders.ToList();
                    //TotalCustomers
                    TotalCustomers = dBContext.Customers.Where(x => x.Accounts.Count >= 1).Count();
                    var TotalCustomersObj = dBContext.Customers.Include(c => c.Accounts).Where(x => x.Accounts.Count >= 1).ToList();
                    var count = dBContext.Customers.Count();
                    //TotalGuests
                    TotalGuests = dBContext.Customers.Count() - TotalCustomers;
                    var TotalGuestsObj = dBContext.Customers.Where(x => x.Accounts != null).ToList();


                    DateTime now = DateTime.Now;

                    //Total guest in month
                    TotalGuestInMonth = await dBContext.Customers.Where(c => c.Orders
                    .Any(o => ((DateTime)o.OrderDate).Year == now.Year &&
                                ((DateTime)o.OrderDate).Month == now.Month)
                                    && c.Accounts.Count == 0).CountAsync();

                    var TotalGuestInMonthObj = dBContext.Customers.Where(c => c.Orders
                    .Any(o => ((DateTime)o.OrderDate).Year == now.Year &&
                                ((DateTime)o.OrderDate).Month == now.Month)
                                    && c.Accounts.Count == 0).ToList();

                    //Total Customer in month
                    TotalCustomerInMonth = await dBContext.Customers.Where(c => c.Orders
                                                .Any(o => ((DateTime)o.OrderDate).Year == now.Year 
                                                    &&((DateTime)o.OrderDate).Month == now.Month)
                                                    &&c.Accounts.Any(a => ((DateTime)a.CreateDate).Year == now.Year
                                                                        && ((DateTime)a.CreateDate).Month == now.Month))
                                                .CountAsync();

                    var TotalCustomerInMonthObj = dBContext.Customers.Where(c => c.Orders
                                                .Any(o => ((DateTime)o.OrderDate).Year == now.Year
                                                    && ((DateTime)o.OrderDate).Month == now.Month)
                                                    && c.Accounts.Any(a => ((DateTime)a.CreateDate).Year == now.Year
                                                                        && ((DateTime)a.CreateDate).Month == now.Month))
                                                .ToList();


                    //OrderInMonth
                    GetOrderInMonthToDashboard(Year);

                    //
                    ViewData["Year"] = Year;
                    return Page();
                }
            }

            return Redirect("/account/login");
        }
         public void GetOrderInMonthToDashboard(int? Year)
        {
            jan = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 1)).Count();
            feb = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 2)).Count();
            mar = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 3)).Count();
            apr = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 4)).Count();
            may = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 5)).Count();
            jun = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 6)).Count();
            jul = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 7)).Count();
            aug = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 8)).Count();
            sep = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 9)).Count();
            oct = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 10)).Count();
            nov = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 11)).Count();
            dec = dBContext.Orders.Where(x => (((DateTime)x.OrderDate).Year == Year) && (((DateTime)x.OrderDate).Month == 12)).Count();
        }
    }
}
