using System.Collections.Generic;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LoadSave.SaveGameHandler;

namespace WebApp.Pages.Game
{
	public class LoadModel : PageModel
	{
		[BindProperty] public List<string> SaveGames { get; set; } = default!;

		public void OnGet()
		{
			SaveGames = GetSaveGameNames();
		}

		public ActionResult OnPost(string? saveName)
		{
			if (!ModelState.IsValid || saveName == null) return Page();

			var savedLevelState = LoadSaveGame(saveName);
			
			
			// reset all id fields and add the state back to db so it is detached from the save
			ResetLevelStateIdFields(savedLevelState);
			var ctx = new AppDbContext();
			ctx.LevelStates.Add(savedLevelState);
			ctx.SaveChangesAsync();

			return RedirectToPage("./Play", new {levelStateId = savedLevelState.LevelStateId});
		}
	}
}