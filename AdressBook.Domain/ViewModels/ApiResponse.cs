using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.ViewModels
{
    public class ApiResponse<TEntity>
    {
        public ApiResponse()
        {
        }

        // Success Response
        public ApiResponse(TEntity data)
        {
            Succeeded = true;
            Data = data;
        }

        // Failed Response
        public ApiResponse(string errorMessage)
        {
            Succeeded = false;
            Message = errorMessage;
        }

        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public TEntity? Data { get; set; }
        public int? StatusCode { get; set; }
        public double? RequestDurationMilliseconds { get; set; }
        public string? ApiVersion { get; set; }
    }
}