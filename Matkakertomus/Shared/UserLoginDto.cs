using System.ComponentModel.DataAnnotations;

namespace Matkakertomus.Shared
{
	public class UserLoginDto
	{
		[EmailAddress]
		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		public string Email { get; set; }
		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Display(Name = "Salasana")]
		public string Password { get; set; }
	}
}
