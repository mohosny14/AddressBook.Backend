using AdressBook.Domain.ViewModels;
using AdressBook.Domain.ViewModels.Common;

namespace AdressBook.Services.IServices
{
    public interface IJobService
    {
        Task<ApiResponse<IEnumerable<GetModelDto>>> GetAllAsync(string? searchtext);

        Task<ApiResponse<string>> CreateAsync(CreateModelDto modelDto);
        Task<ApiResponse<GetModelDto>> GetById(int Id);
        Task<ApiResponse<string>> SoftDelete(int Id);
        Task<ApiResponse<string>> UpdateAsync(UpdateModelDto modelDto);
    }
}