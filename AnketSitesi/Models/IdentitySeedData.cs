using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace AnketSitesi.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
		
		private const string adminPassword = "Admin_123";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context =app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<Context>();


            if (context.Database.GetPendingMigrations().Any() )
                {
                context.Database.Migrate();
            }

            var userManager =app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

           var user= await userManager.FindByNameAsync(adminUser);

            if (user == null) 
            {
                user = new AppUser
                { UserName = adminUser,  FullName="Halit kerem", Email = "admin@gmail.com", PhoneNumber = "44444444" ,EmailConfirmed=true };

                await userManager.CreateAsync(user,adminPassword);
            }
           
            
           
        }

    }
}
