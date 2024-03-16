using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBoostarpApp1.Models
{
    public class RegisterModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
