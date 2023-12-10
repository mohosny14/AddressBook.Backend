using AdressBook.Domain.IRepositories;
using AdressBook.Domain.Models;
using AdressBook.Domain.ViewModels;
using AdressBook.Domain.ViewModels.Employee;
using AdressBook.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace AdressBook.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmployeeService(IEmployeeRepository employeeRepository,
                               IMapper mapper,
                               IUnitOfWork unitOfWork,
                               IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ApiResponse<string>> CreateAsync(CreateEmployeeDto employeeDto)
        {
            #region validation
            string emailpattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            bool isEmailMatch = Regex.IsMatch(employeeDto.Email, emailpattern);

            if (!isEmailMatch)
            {
                return new ApiResponse<string>() { Message = "Please Enter Valid E-mail", Succeeded = false };
            }
            string Mobilepattern = @"^01[1052][0-9]{8}$";
            bool isMobileMatch = Regex.IsMatch(employeeDto.MobileNumber, Mobilepattern);

            if (!isMobileMatch)
            {
                return new ApiResponse<string>() { Message = "Please Enter Valid Phone Number", Succeeded = false };
            }
            #endregion

            try
            {
                string filePath = "";

                if (employeeDto.Photo != null)
                {
                    filePath = await UploadPhoto(employeeDto.Photo);
                }
                var employee = _mapper.Map<Employee>(employeeDto);

                employee.PhotoUrl = filePath;

                await _employeeRepository.AddAsync(employee);
                _unitOfWork.Complete();
                return new ApiResponse<string>()
                {
                    Message = "Added Successfully!",
                    Succeeded = true
                };
            }
            catch
            {
                return new ApiResponse<string>()
                {
                    Message = "something wrong happened!",
                    Succeeded = false
                };
            }


        }
        private async Task<string> UploadPhoto(IFormFile image)
        {
            string uniqueFileName = GetUniqueFileName(image.FileName);
            string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return filePath;
        }
        private string GetUniqueFileName(string fileName)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);
            string randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 10);
            string uniqueFileName = $"{fileNameWithoutExtension}_{randomString}{fileExtension}";

            return uniqueFileName;
        }

        public async Task<ApiResponse<IEnumerable<GetEmployeeDto>>> GetAllAsync(string? searchtext)
        {
            string[] includes = new string[] { "Job", "Department" };

            var searchResults = await _employeeRepository.FindAllAsync(
                                                                       e => ((string.IsNullOrEmpty(searchtext) ||
                                                                       e.FullName.Contains(searchtext) ||
                                                                       e.Email.Contains(searchtext) ||
                                                                       e.MobileNumber.Contains(searchtext)) && !e.IsDeleted), includes);

            if (searchResults != null)
            {
                var employees = _mapper.Map<List<GetEmployeeDto>>(searchResults);
                return new ApiResponse<IEnumerable<GetEmployeeDto>>()
                {
                    Succeeded = true,
                    Data = employees
                };
            }
            else
            {
                return new ApiResponse<IEnumerable<GetEmployeeDto>>()
                {
                    Succeeded = false,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<GetEmployeeDto>> GetById(int Id)
        {
            string[] includes = new string[] { "Job", "Department" };

            var result = await _employeeRepository.FindAsync( e => e.Id == Id && !e.IsDeleted, includes);

            if (result != null)
            {
                var employee = _mapper.Map<GetEmployeeDto>(result);
                return new ApiResponse<GetEmployeeDto>()
                {
                    Succeeded = true,
                    Data = employee
                };
            }
            else
            {
                return new ApiResponse<GetEmployeeDto>()
                {
                    Succeeded = false,
                    Data = null,
                    Message = "This Employee Not Found"
                };
            }
        }

        public async Task<ApiResponse<string>> SoftDelete(int Id)
        {
            var employee = await _employeeRepository.FindAsync(e => e.Id == Id && !e.IsDeleted);
            if (employee == null)
            {
                return new ApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid Employee",
                    Succeeded = false
                };
            }
            employee.IsDeleted = true;
            _employeeRepository.Update(employee);
            _unitOfWork.Complete();

            return new ApiResponse<string>()
            {
                Data = null,
                Message = "Deleted Successfully!",
                Succeeded = true
            };
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateEmployeeDto employeeDto)
        {
            #region validation
            string emailpattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            bool isEmailMatch = Regex.IsMatch(employeeDto.Email, emailpattern);

            if (!isEmailMatch)
            {
                return new ApiResponse<string>() { Message = "Please Enter Valid E-mail", Succeeded = false };
            }
            string Mobilepattern = @"^01[1052][0-9]{8}$";
            bool isMobileMatch = Regex.IsMatch(employeeDto.MobileNumber, Mobilepattern);

            if (!isMobileMatch)
            {
                return new ApiResponse<string>() { Message = "Please Enter Valid Phone Number", Succeeded = false };
            }
            #endregion

            var employee = await _employeeRepository.FindAsync(c => c.Id == employeeDto.Id && !c.IsDeleted);

            if (employee is null)
            {
                return new ApiResponse<string>()
                {
                    Succeeded = false,
                    Data = null,
                    Message = "Invalid ID"
                };
            }
            string filePath = "";

            if (employeeDto.Photo != null)
            {
                filePath = await UploadPhoto(employeeDto.Photo);
            }

            employee = _mapper.Map<Employee>(employeeDto);
            employee.PhotoUrl = filePath;

            _employeeRepository.Update(employee);
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