using BankSystem.App.Dto;
using AutoMapper;
using BankSystem.Domain.Models;

namespace BankSystem.App
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

            CreateMap<ClientDto, Client>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetName(src.FullName)))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => GetSurname(src.FullName)));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Name} {src.Surname}"));

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetName(src.FullName)))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => GetSurname(src.FullName)));
        }

        private string GetName(string fullname)
        {
            var names = fullname.Split(' ', 2);
            return names.Length > 0 ? names[0] : string.Empty;
        }

        private string GetSurname(string fullname) 
        {
            var names = fullname.Split(" ", 2);
            return names.Length > 0 ? names[1] : string.Empty;
        }
    }
}
