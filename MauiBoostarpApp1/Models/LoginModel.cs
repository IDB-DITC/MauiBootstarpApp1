
using System.ComponentModel.DataAnnotations;

namespace MauiBoostarpApp1.Models
{
    public class LoginModel
    {
		[Required, EmailAddress]
		public string? Email { get; set; }
		[Required, DataType(DataType.Password)]
		public string? Password { get; set; }
	}
}
