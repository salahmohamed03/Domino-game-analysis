using Microsoft.EntityFrameworkCore;

namespace MauiApp1.Data
{
	public class AppDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			string conDB = $"Filename={PathDB.GetDatabasePath("test.db")}";
			object value = optionsBuilder.UseSqlite(conDB);
		}

	}
}
