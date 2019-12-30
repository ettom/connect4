using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace LoadSave
{
	public static class SaveGameHandler

	{
		public static void DeleteSaveGame(string name)
		{
			var ctx = new AppDbContext();
			var saveGame = ctx.SaveGames.SingleOrDefault(x => x.Name == name);
			if (saveGame == null) {
				return;
			}

			ctx.SaveGames.RemoveRange(ctx.SaveGames.Where(x => x.Name == name));

			ctx.SaveChanges();
		}

		public static async Task SaveLevelState(LevelState state, string name)
		{
			var ctx = new AppDbContext();
			await ctx.SaveChangesAsync(); // sync all states
			DeleteSaveGame(name);

			state = ResetLevelStateIdFields(state);

			var save = new SaveGame {Name = name, LevelState = state};

			await ctx.SaveGames.AddAsync(save);
			await ctx.SaveChangesAsync();
		}

		public static LevelState LoadSaveGame(string name)
		{
			var ctx = new AppDbContext();
			var saveGame = ctx.SaveGames.AsNoTracking().Include(s => s.LevelState)
				.ThenInclude(l => l.Players)
				.ThenInclude(p => p.Discs)
				.Single(s => s.Name == name).LevelState;

			if (saveGame == null) {
				throw new SaveGameNotFoundException();
			}

			return saveGame;
		}


		public static LevelState RestoreGameStateFromDb(int levelStateId)
		{
			var ctx = new AppDbContext();
			var savedLevelState = ctx.LevelStates
				.Include(l => l.Players)
				.ThenInclude(p => p.Discs)
				.Single(a => a.LevelStateId == levelStateId);

			return savedLevelState;
		}

		public static async Task UpdateLevelStateInDb(LevelState state)
		{
			var ctx = new AppDbContext();
			ctx.LevelStates.Update(state);
			await ctx.SaveChangesAsync();
			
		}

		public static LevelState ResetLevelStateIdFields(LevelState toClone)
		{
			var ctx = new AppDbContext();
			foreach (var player in toClone.Players) {
				foreach (var disc in player.Discs) {
					ctx.Entry(disc).State = EntityState.Detached;
					disc.DiscId = 0;
					disc.PlayerId = 0;
				}

				ctx.Entry(player).State = EntityState.Detached;
				player.PlayerId = 0;
				player.LevelStateId = 0;
			}

			toClone.SaveGame = null;
			toClone.SaveGameId = null;

			ctx.Entry(toClone).State = EntityState.Detached;
			toClone.LevelStateId = 0;

			return toClone;
		}

		public static async Task AddGameStateToDb(LevelState state)
		{
			var ctx = new AppDbContext();
			await ctx.LevelStates.AddAsync(state);
			await ctx.SaveChangesAsync();
		}


		public static List<string> GetSaveGameNames()
		{
			var ctx = new AppDbContext();
			return ctx.SaveGames.Select(saveGame => saveGame.Name).ToList();
		}

		private class SaveGameNotFoundException : Exception
		{
		}
	}
}