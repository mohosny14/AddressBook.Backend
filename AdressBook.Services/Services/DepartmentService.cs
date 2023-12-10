using AdressBook.Domain.IRepositories;
using AdressBook.Domain.Models;
using AdressBook.Domain.ViewModels;
using AdressBook.Domain.ViewModels.Common;
using AdressBook.Infrastructure.Repositories;
using AdressBook.Services.IServices;
using AutoMapper;

namespace AdressBook.Services.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IDepartmentRepository departmentRepository,
                                IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CreateAsync(CreateModelDto modelDto)
        {
            var department = _mapper.Map<Department>(modelDto);
            if (department != null)
            {
                await _departmentRepository.AddAsync(department);
                await _unitOfWork.CompleteAsync();

                return new ApiResponse<string>()
                {
                    Data = null,
                    Succeeded = true,
                    Message = "Added Successfully"
                };
            }
            return new ApiResponse<string>()
            {
                Message = "something wrong happened!",
                Succeeded = false
            };
        }

        public async Task<ApiResponse<IEnumerable<GetModelDto>>> GetAllAsync(string? searchtext)
        {
            var searchResults = await _departmentRepository.FindAllAsync(
                                                           e => ((string.IsNullOrEmpty(searchtext) ||
                                                           e.Name.Contains(searchtext) ||
                                                           e.Description.Contains(searchtext)) && !e.IsDeleted));

            if (searchResults != null)
            {
                var employees = _mapper.Map<List<GetModelDto>>(searchResults);
                return new ApiResponse<IEnumerable<GetModelDto>>()
                {
                    Succeeded = true,
                    Data = employees
                };
            }
            else
            {
                return new ApiResponse<IEnumerable<GetModelDto>>()
                {
                    Succeeded = false,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<GetModelDto>> GetById(int Id)
        {
            var result = await _departmentRepository.FindAsync(j => !j.IsDeleted && j.Id == Id);
            if (result != null)
            {
                var department = _mapper.Map<GetModelDto>(result);
                return new ApiResponse<GetModelDto>()
                {
                    Succeeded = true,
                    Data = department
                };
            }
            else
            {
                return new ApiResponse<GetModelDto>()
                {
                    Succeeded = false,
                    Data = null,
                    Message = "Not Found"
                };
            }
        }

        public async Task<ApiResponse<string>> SoftDelete(int Id)
        {
            var department = await _departmentRepository.FindAsync(e => e.Id == Id && !e.IsDeleted);
            if (department == null)
            {
                return new ApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid Employee",
                    Succeeded = false
                };
            }
            department.IsDeleted = true;
            _departmentRepository.Update(department);
            _unitOfWork.Complete();

            return new ApiResponse<string>()
            {
                Data = null,
                Message = "Deleted Successfully!",
                Succeeded = true
            };
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateModelDto modelDto)
        {
            var department = await _departmentRepository.FindAsync(e => e.Id == modelDto.Id && !e.IsDeleted);
            if (department == null)
            {
                return new ApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid Employee",
                    Succeeded = false
                };
            }
            department = _mapper.Map<Department>(modelDto);

            _departmentRepository.Update(department);
            _unitOfWork.Complete();

            return new ApiResponse<string>()
            {
                Data = null,
                Succeeded = true,
                Message = "Updated Successfully"
            };
        }

    }
}