using System.ComponentModel.DataAnnotations;

namespace AnketSitesi.ViewModels
{
	public class EditViewModel
	{

		public string? Id { get; set; } 
        public string? UserName { get; set; } 

			
			public string? FullName { get; set; }


		   [Display(Name = "Email adresi")]
		  [Required(ErrorMessage = "Email adresi gereklidir")]
	        [EmailAddress(ErrorMessage = "Hatalı Email Adresi")]
		   public string Email { get; set; }




        [DataType(DataType.Password)]
			public string? Password { get; set; } = string.Empty;

			
			[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
			public string ?ConfirmPassword { get; set; } = string.Empty;

		public IList<string>? SelectedRoles { get; set; }


		
	}
	}