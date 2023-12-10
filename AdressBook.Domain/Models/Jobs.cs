using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.Models
{
    public class Jobs : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}