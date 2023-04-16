using AutoMapper;
using SneakerOnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
namespace SneakerOnlineShop.DTO
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.PresentImage, opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault().ImagePath));
            CreateMap<ProductDTO, Product>();
        }
    }
}
