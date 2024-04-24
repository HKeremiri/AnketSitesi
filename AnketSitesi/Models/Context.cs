using AnketSitesi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Context : IdentityDbContext<AppUser, AppRole, string>
{
	public Context(DbContextOptions<Context> options)
		: base(options)
	{
	}

    public DbSet<Anket> Ankets { get; set; }

	public DbSet<Sorular> Sorulars { get; set; }
 
    public DbSet<Cevaplar> Cevaplars { get; set; }

	public DbSet<Secenekler> Seceneklers { get; set; }

	public DbSet<CevaplamaDurumu> CevaplamaDurumus { get; set; }	
}


