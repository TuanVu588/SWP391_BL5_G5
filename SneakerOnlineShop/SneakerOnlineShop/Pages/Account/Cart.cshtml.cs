using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SneakerOnlineShop.DTO;
using SneakerOnlineShop.Entities;
using SneakerOnlineShop.Helpers;
using SneakerOnlineShop.Models;
using System.Collections.Generic;
using System.Text;

namespace SneakerOnlineShop.Pages.Account
{
    public class CartModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        IMapper _mapper;
        public List<Item> cart { get; set; }
        public double Total { get; set; }

        public CartModel(SWP391_DBContext dBContext, IMapper mapper)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }

        public void OnGet()
        {
            cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            Total = (double)cart.Sum(i => i.ProductDTO.UnitPrice * i.Quantity);
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
                if (cart[i].ProductDTO.ProductId == proid )
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
            ProductDTO proDto =(ProductDTO)_mapper.Map<ProductDTO>(pro);
            return proDto;
        }
    }
}
