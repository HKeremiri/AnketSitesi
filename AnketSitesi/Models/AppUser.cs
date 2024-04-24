using Microsoft.AspNetCore.Identity;

namespace AnketSitesi.Models
{
	public class AppUser:IdentityUser
	{
		public string ? FullName { get; set; } 
    }
}
