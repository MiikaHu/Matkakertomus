using System.ComponentModel.DataAnnotations;

namespace Matkakertomus.Shared
{
	public class UserRegisterDto
	{
		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Display(Name = "Etunimi")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Display(Name = "Sukunimi")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Display(Name = "Käyttäjätunnus")]
		public string Username { get; set; }

		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Display(Name = "Salasana")]
		public string Password { get; set; }

		[Required(ErrorMessage = "{0} on pakollinen tieto")]
		[Compare(nameof(Password))]
		[Display(Name = "Salasanan varmistus")]
		public string ConfirmPassword { get; set; }
	}
}
