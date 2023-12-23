using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.User_Service.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression ("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*\\W).{6,}$" , ErrorMessage = "Password must be at least 6 characters long with 1 uppercase, 1 lowercase, 1 number, and 1 non-alphanumeric character")]
        public string Password { get; set; }
    }
}
