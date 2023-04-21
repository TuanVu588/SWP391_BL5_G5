using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Employee.Order
{
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;

        public List<Models.Order> orders { get; set; }
        public IndexModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }
        public async Task<IActionResult> OnGet(int eid, string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                getAllOrder(eid);

            }
            else
            {
                OnGetFilterOrder(fromDate, toDate);
            }
            return Page();
        }

        private void getAllOrder(int eid)
        {
            ViewData["eid"] = eid;
            orders = (List<Models.Order>)_dbContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Customer).ToList();
        }

        public async void OnGetFilterOrder(string fromDate, string toDate)
        {
            orders = _dbContext.Orders.Include(o => o.Employee)
                    .Include(o => o.Customer).Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages).ToList();

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
            //return Page();
        }
    }
}
