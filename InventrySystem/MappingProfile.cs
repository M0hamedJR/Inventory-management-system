using AutoMapper;
using Entities.Identity;
using Entities.Models;
using Shared.DTO.Category;
using Shared.DTO.Warehouse;
using Shared.DTO.Product;
using Shared.DTO.User;
using static Shared.DTO.Shelf.Shelf;

namespace InventrySystem
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UserForRegistrationDto, User>()
                        .ForMember(u => u.UserName, opt => opt.MapFrom(x => GenerateValidUserName(x.Email)));

            CreateMap<Product, ProductDto>();
            CreateMap<ProductForCreationDto, Product>();
            CreateMap<ProductForUpdateDto, Product>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryForCreationDto, Category>();
            CreateMap<CategoryForUpdateDto, Category>();

            CreateMap<Shelf, ShelfDto>();
            CreateMap<ShelfForCreationDto, Shelf>();
            CreateMap<ShelfForUpdateDto, Shelf>();

            CreateMap<Warehouse, WarehouseDto>();
            CreateMap<WarehouseForCreationDto, Warehouse>();
            CreateMap<WarehouseForUpdateDto, Warehouse>();

            CreateMap<UserForRegistrationDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserRoleForCreationDto, UserRole>();
            CreateMap<UserRoleForUpdateDto, UserRole>();
        }
        private string GenerateValidUserName(string email)
        {
            var atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                return email.Substring(0, atIndex);
            }
            return email;
        }
    }
}
