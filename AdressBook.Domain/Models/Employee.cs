using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.Models
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhotoUrl { get; set; }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Year;

                if (BirthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public int JobId { get; set; }
        public int DepartmentId { get; set; }

        public Jobs Job { get; set; }
        public Department Department { get; set; }
    }
}