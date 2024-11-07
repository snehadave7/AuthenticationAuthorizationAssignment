using AuthenticationDemo.Models;
using AutoMapper;

namespace AuthenticationDemo.Mappings {
    public class MappingProfile:Profile {
        public MappingProfile()
        {
            CreateMap<Products, ProductDTO>().ReverseMap();

        }
    }
}
