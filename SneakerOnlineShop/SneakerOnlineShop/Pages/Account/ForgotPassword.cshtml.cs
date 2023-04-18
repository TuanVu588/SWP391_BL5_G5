using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SneakerOnlineShop.Models;

namespace SneakerOnlineShop.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {

        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;
        public double Total { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Order Order { get; set; }

        public ForgotPasswordModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }
    }
}
