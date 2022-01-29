using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.ViewModels
{
    public class UserAndRoleVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required!")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
