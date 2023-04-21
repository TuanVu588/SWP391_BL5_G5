using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Admin.Order
{
    
    public class IndexModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;

        public List<SneakerOnlineShop.Models.Order> orders { get; set; }
        //public SneakerOnlineShop.Models.Employee employee { get; set; }
        //public SneakerOnlineShop.Models.Order order { get; set; }
        public IndexModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet(int eid, string fromDate, string toDate)
        {
            //int employeeId = int.Parse(eid);
            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                getAllOrder(eid);

            }
            else
            {
                OnGetFilterOrder(fromDate, toDate, eid.ToString());
            }
            return Page();
        }

        private void getAllOrder(int eid)
        {
            ViewData["eid"] = eid;
            orders = (List<SneakerOnlineShop.Models.Order>)_dbContext.Orders
                    .Include(o => o.Employee)
                    .Include(o => o.Customer).ToList();
        }

        // order status: complete
        public async Task<IActionResult> OnPostUpdateOrder(String shipDate, String orderId, String employeeId)
        {
            int eid = Int32.Parse(employeeId);
            getAllOrder(eid);
            int oid = Int32.Parse(orderId);
            DateTime shippedDate = DateTime.Parse(shipDate);

            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == oid);

            if (DateTime.Compare(DateTime.Now, shippedDate) > 0
                || DateTime.Compare(shippedDate, (DateTime)order.RequiredDate) > 0)
            {
                ViewData["Error"] = "RequireDate must be greatter than today";
                return Page();
            }
            if (order != null)
            {
                order.ShippedDate = shippedDate;
                order.EmployeeId = eid;
                order.Status = "Complete";
                _dbContext.SaveChanges();
            }
            return RedirectToPage("Index", new { eid = eid });
        }
        //reject order: reject
        public async Task<IActionResult> OnGetReject(String employeeId, String orderId)
        {
            int oid = Int32.Parse(orderId);
            int eid = Int32.Parse(employeeId);
            getAllOrder(eid);

            //update lai status
            var order = _dbContext.Orders.FirstOrDefault(o => o.OrderId == oid);
            if (order != null)
            {
                order.Status = "Reject";
                _dbContext.SaveChanges();
            }
            return Page();
        }
        public async Task<IActionResult> OnGetFilterOrder(string fromDate, string toDate, String eid)
        {
            int employeeId = int.Parse(eid);
            ViewData["eid"] = employeeId;
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
            return Page();
        }

    }
}
