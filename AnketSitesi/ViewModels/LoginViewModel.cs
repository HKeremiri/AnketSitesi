using System.ComponentModel.DataAnnotations;

namespace AnketSitesi.ViewModels
{
	public class LoginViewModel
	{
        [Required]
        
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }

        
    }
}
