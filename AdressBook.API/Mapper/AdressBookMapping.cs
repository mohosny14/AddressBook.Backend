using AdressBook.Domain.Models;
using AdressBook.Domain.ViewModels.Common;
using AdressBook.Domain.ViewModels.Employee;
using AutoMapper;

namespace AdressBook.API.Mapper
{
    public class AdressBookMapping : Profile
    {
        public AdressBookMapping()
        {
            //Employee
            CreateMap<Employee, CreateEmployeeDto>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Employee, UpdateEmployeeDto>()
           .ForMember(dest => dest.Photo, opt => opt.Ignore())
           .ReverseMap();

            CreateMap<Employee, GetEmployeeDto>()
                 .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                 .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.Job.Name))
                .ReverseMap();

            //Department
            CreateMap<Department, GetModelDto>()
               .ReverseMap();
            CreateMap<Department, UpdateModelDto>()
               .ReverseMap();
            CreateMap<Department, CreateModelDto>()
               .ReverseMap();

            //Jobs
            CreateMap<Jobs, GetModelDto>()
              .ReverseMap();

            CreateMap<Jobs, UpdateModelDto>()
                .ReverseMap();

            CreateMap<Jobs, CreateModelDto>()
               .ReverseMap();

        }
    }
}