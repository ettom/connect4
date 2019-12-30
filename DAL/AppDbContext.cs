using System;
using System.Linq;
using Core;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
	public class AppDbContext : DbContext
	{
		public DbSet<LevelState> LevelStates { get; set; } = default!;
		public DbSet<Player> Players { get; set; } = default!;
		public DbSet<Disc> Discs { get; set; } = default!;
		public DbSet<SaveGame> SaveGames { get; set; } = default!;
		public DbSet<Settings> Settings { get; set; } = default!;
		public DbSet<PlayerPrototype> PlayerPrototypes { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			foreach (var relationship
				in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
				relationship.DeleteBehavior = DeleteBehavior.Cascade;
			}
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var dbPath = Environment.GetEnvironmentVariable("PWD") + "/data.db";
			if (string.IsNullOrEmpty(dbPath)) {
				throw new Exception("'connect4db' env variable must be set!.");
			}

			optionsBuilder.UseSqlite($"Data Source={dbPath};");
			optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();
		}
	}
}
