using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Account
{
    public class OrderStatusModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;

        public List<Order> orders { get; set; }
        public OrderStatusModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }
        public async Task OnGet(String cusid)// order cua cusnao
        {
            //lay order cua cusdo
            orders = _dbContext.Orders
                .Where(o => o.CustomerId.Equals(cusid))
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductImages).ToList();
        }
    }
}
