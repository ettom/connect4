using System.Threading.Tasks;
using Core;
using GameEngine;
using LoadSave;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LoadSave.SaveGameHandler;

namespace WebApp.Pages.Game
{
	public class PlayModel : PageModel
	{
		public LevelState LevelState { get; set; } = default!;


		public async Task<ActionResult> OnGet(int? levelStateId, int? col)
		{
			if (levelStateId == null) {
				var settings = SettingsHandler.GetSavedSettingsElseNew();

				LevelState = new LevelState(settings);
				await AddGameStateToDb(LevelState);
			} else {
				LevelState = RestoreGameStateFromDb((int) levelStateId);
			}

			// don't allow moves if there is a win or a tie
			if (LevelState.IsGameOver()) {
				return Page();
			}

			if (col != null) {
				LevelState = LevelUpdate.UpdateLevel(LevelState, LevelState.Turn, (int) col);
			}

			while (LevelState.GetCurrentPlayer().isAI) {
				LevelState = LevelUpdate.UpdateLevel(LevelState, LevelState.Turn);
			}

			await UpdateLevelStateInDb(LevelState);

			return Page();
		}
	}
}