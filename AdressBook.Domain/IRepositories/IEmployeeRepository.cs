using AdressBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
    }
}