using AnketSitesi.Models;
using System.ComponentModel.DataAnnotations;

namespace AnketSitesi.ViewModels
{
	public class RegisterViewModel
	{
		
		[Required(ErrorMessage ="Kullanıcı adı boş bırakılmaz")]
		
		public string UserName { get; set; } = string.Empty;

		[Required(ErrorMessage = "İsim boş bırakılmaz")]
		public string FullName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email boş Bırakılmaz")]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		

		[Required(ErrorMessage ="Şifre Boş bırakılmaz")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage ="Şifrenizi Tekrar giriniz")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor")]
		public string ConfirmPassword { get; set; } = string.Empty;


	}
}
