using AdressBook.Domain.ViewModels;
using AdressBook.Domain.ViewModels.Employee;

namespace AdressBook.Services.IServices
{
    public interface IEmployeeService
    {
        Task<ApiResponse<string>> CreateAsync(CreateEmployeeDto employeeDto);
        Task<ApiResponse<IEnumerable<GetEmployeeDto>>> GetAllAsync(string? searchtext);
        Task<ApiResponse<GetEmployeeDto>> GetById(int Id);
        Task<ApiResponse<string>> SoftDelete(int Id);
        Task<ApiResponse<string>> UpdateAsync(UpdateEmployeeDto employeeDto);


    }
}