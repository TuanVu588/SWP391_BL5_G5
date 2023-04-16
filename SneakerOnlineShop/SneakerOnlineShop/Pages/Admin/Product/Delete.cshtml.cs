using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using SneakerOnlineShop.Hubs;
using SneakerOnlineShop.Models;
using Microsoft.EntityFrameworkCore;

namespace SneakerOnlineShop.Pages.Admin.Product
{
    public class DeleteModel : PageModel
    {
        private readonly SWP391_DBContext dBContext;
        private readonly IHubContext<SignalRServer> context;
        
        public DeleteModel(SWP391_DBContext dBContext, IHubContext<SignalRServer> context)
        {
            this.dBContext = dBContext;
            this.context = context;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var product = await dBContext.Products.Where(p => p.ProductId == id).FirstOrDefaultAsync();
            if(product != null)
            {
                List<OrderDetail> orderDetails = await dBContext.OrderDetails.Where(o => o.ProductId == product.ProductId).ToListAsync();
                dBContext.OrderDetails.RemoveRange(orderDetails);
                dBContext.Products.Remove(product);
                dBContext.SaveChanges();
            }
            return RedirectToPage("/admin/product/index");
        }
    }
}
