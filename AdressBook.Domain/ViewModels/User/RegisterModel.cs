using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.ViewModels.User
{
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string UserName { get; set; }



        [Required, StringLength(100)]
        //[EmailAddress(ErrorMessage = "أدخل بريد صحيح")]
        public string Email { get; set; }


        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [Required, StringLength(100)]
        public string PhoneNumber { get; set; }
    }
}