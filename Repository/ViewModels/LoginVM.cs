using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "is required!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "is required!")]
        public string Password { get; set; }
    }
}
