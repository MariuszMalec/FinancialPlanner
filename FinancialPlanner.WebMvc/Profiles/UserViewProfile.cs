using AutoMapper;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.WebMvc.Profiles
{
    public class UserViewProfile : Profile
    {
        public UserViewProfile()
        {
            //TODO dodanie roli do view
            CreateMap<User, UserDto>()
                //.ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RoleName, o => o.MapFrom(s => $"{s.Role.Name}"))
                ;
        }
    }
}
