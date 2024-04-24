using System.ComponentModel.DataAnnotations;

namespace AnketSitesi.ViewModels
{
    public class ResetPasswordModel
    {

        [Required]
        public string Token { get; set; }=string.Empty;

        [Required(ErrorMessage = "Email boş Bırakılmaz")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre Boş bırakılmaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrenizi Tekrar giriniz")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
