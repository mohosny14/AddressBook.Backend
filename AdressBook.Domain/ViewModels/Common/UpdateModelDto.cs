using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.ViewModels.Common
{
    public class UpdateModelDto : CreateModelDto
    {
        public int Id { get; set; }
    }
}