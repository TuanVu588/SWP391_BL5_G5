using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.DTO;
using SneakerOnlineShop.Entities;
using SneakerOnlineShop.Helpers;
using SneakerOnlineShop.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace SneakerOnlineShop.Pages.Account
{
    public class CartModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;
        public List<Item>? cart { get; set; }
        public double Total { get; set; }
        [BindProperty]
        public Customer Customer { get; set; }
        [BindProperty]
        public Order Order { get; set; }

        public CartModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }

        public async Task OnGet()
        {
            setCustomerIfHaveSession();
            setCart();
        }

        public async Task<IActionResult> OnPost()
        {
            //To return Page();
            setCustomerIfHaveSession();
            setCart();
            //String successMsg = null;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (DateTime.Compare(DateTime.Now, (DateTime)Order.RequiredDate) > 0)
            {
                ViewData["Error"] = "RequireDate must be greatter than today";
                return Page();
            }
            //Add to order
            //Neu la khach moi thi customer, add order, order detail
            if (cart != null)
            {
                if (Customer.CustomerId.Contains("-1"))//Customer new
                {
                    //add customer
                    Customer cus = new Customer();
                    cus.CustomerId = RandomCustomerId(5);
                    cus.ContactName = Customer.ContactName;
                    cus.CompanyName = Customer.CompanyName;
                    cus.ContactTitle = Customer.ContactTitle;
                    cus.Address = Customer.Address;
                    cus.Phone = Customer.Phone;
                    cus.CustomerName = Customer.CustomerName;
                    cus.Status = 1;
                    //save customer with order with order details
                    cus.Orders.Add(addOrderForCustomer(cus));
                    _dbContext.Customers.Add(cus);
                    _dbContext.SaveChanges();
                    //successMsg = "Order successfull --> go to profile to check order status";
                }
                else  //neu la khach cu --> add order theo customerId, order detail
                {
                    //save customer
                    var cus = _dbContext.Customers.SingleOrDefault(cus => cus.CustomerId.Equals(Customer.CustomerId));
                    if (cus != null)
                    {
                        cus.ContactName = Customer.ContactName;
                        cus.CompanyName = Customer.CompanyName;
                        cus.ContactTitle = Customer.ContactTitle;
                        cus.Address = Customer.Address;
                        cus.Phone = Customer.Phone;
                        cus.CustomerName = Customer.CustomerName;
                        cus.Orders.Add(addOrderForCustomer(cus));
                        _dbContext.SaveChanges();
                        //successMsg = "Order successfull --> go to profile to check order status";
                    }
                }
            }
            SessionHelper.removeSession(HttpContext.Session, "cart");
            return RedirectToPage("Cart");
        }
        public IActionResult OnGetBuyNow(int proid)
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>();
                cart.Add(new Item
                {
                    ProductDTO = findProduct(proid),
                    Quantity = 1
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = Exists(cart, proid);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        ProductDTO = findProduct(proid),
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToPage("Cart");
        }

        public IActionResult OnGetDelete(int proid)
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = Exists(cart, proid);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToPage("Cart");
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (var i = 0; i < cart.Count; i++)
            {
                cart[i].Quantity = quantities[i];
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToPage("Cart");
        }

        private int Exists(List<Item> cart, int proid)
        {
            for (var i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductDTO.ProductId == proid)
                {
                    return i;
                }
            }
            return -1;
        }
        public string RandomCustomerId(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
        public ProductDTO findProduct(int id)
        {
            Product pro = _dbContext.Products.Include(p => p.ProductImages).Where(p => p.ProductId == id).SingleOrDefault();
            ProductDTO proDto = (ProductDTO)_mapper.Map<ProductDTO>(pro);
            return proDto;
        }
        private void setCustomerIfHaveSession()
        {
            SneakerOnlineShop.Models.Account accSession = new SneakerOnlineShop.Models.Account();
            if (HttpContext.Session.GetString("account") != null)
            {
                accSession = JsonSerializer.Deserialize<SneakerOnlineShop.Models.Account>(HttpContext.Session.GetString("account"));
            }
            if (accSession != null && accSession.RoleId == 1)
            {
                String cid = accSession.CustomerId;// customer model must not be null
                Customer = _dbContext.Customers.Where(c => c.CustomerId.Equals(cid)).SingleOrDefault();
            }
        }
        private void setCart()
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart != null) { Total = (double)cart.Sum(i => i.ProductDTO.UnitPrice * i.Quantity); }

        }
        private Order addOrderForCustomer(Customer cus)
        {
            //add order cung voi customer
            Order newOrder = new Order();
            newOrder.OrderDate = DateTime.Now;
            newOrder.ShipAddress = Order.ShipAddress;
            newOrder.ShipCity = Order.ShipCity;
            newOrder.RequiredDate = Order.RequiredDate;
            newOrder.Status = "Pending";
            //add order detail
            foreach (var i in cart)
            {
                OrderDetail od = new OrderDetail();
                od.ProductId = i.ProductDTO.ProductId;
                od.UnitPrice = (decimal)i.ProductDTO.UnitPrice;
                od.Quantity = (short)i.Quantity;
                newOrder.OrderDetails.Add(od);
            }
            return newOrder;
        }
    }
}
