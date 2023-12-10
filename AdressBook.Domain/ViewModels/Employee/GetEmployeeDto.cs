using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.ViewModels.Employee
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhotoUrl { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDate { get; set; }

        public string JobName { get; set; }
        public string DepartmentName { get; set; }
    }
}