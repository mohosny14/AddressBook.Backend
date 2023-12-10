using AdressBook.Domain.ViewModels.User;
using AdressBook.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Services.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthModel>> Login(LoginModel model);
        Task<ApiResponse<AuthModel>> RegisterAsync(RegisterModel model);
    }
}