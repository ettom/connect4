using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace LoadSave
{
	public static class SettingsHandler
	{
		public static Settings GetSavedSettingsElseNew()
		{
			try {
				return LoadSettings();
			} catch (SettingsNotFoundException) {
				return new Settings(false);
			}
		}

		public static async Task SaveSettings(Settings settings)
		{
			var ctx = new AppDbContext();

			await ctx.SaveChangesAsync();
			settings = UntrackSettings(settings);

			ctx.Settings.RemoveRange(ctx.Settings);
			ctx.PlayerPrototypes.RemoveRange(ctx.PlayerPrototypes);

			await ctx.SaveChangesAsync();


			if (settings.Width == 0 || settings.Turn == 0 || settings.Height == 0 ||
			    settings.PlayerPrototypes.Any(p => p.Number == 0)) {
				// sanity check
				throw new Exception("bad save!");
			}


			await ctx.Settings.AddAsync(settings);
			await ctx.SaveChangesAsync();
		}

		public static Settings LoadSettings()
		{
			var ctx = new AppDbContext();
			var tmp = ctx.Settings.AsNoTracking()
				.Include(s => s.PlayerPrototypes)
				.FirstOrDefault();
			if (tmp == null) {
				throw new SettingsNotFoundException();
			}

			return tmp;
		}


		private static Settings UntrackSettings(Settings toClone)
		{
			var ctx = new AppDbContext();
			foreach (var player in toClone.PlayerPrototypes) {
				ctx.Entry(player).State = EntityState.Detached;
				player.PlayerPrototypeId = 0;
				player.SettingsId = 0;
			}

			ctx.Entry(toClone).State = EntityState.Detached;
			toClone.SettingsId = 0;

			return toClone;
		}

		private class SettingsNotFoundException : Exception
		{
		}
	}
}