using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.ViewModels.Employee
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //public string PhotoUrl { get; set; }
        public IFormFile Photo { get; set; }

        public int JobId { get; set; }
        public int DepartmentId { get; set; }
    }
}