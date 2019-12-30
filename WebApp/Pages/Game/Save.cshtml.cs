using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LoadSave.SaveGameHandler;

namespace WebApp.Pages.Game
{
	public class SaveModel : PageModel
	{
		[BindProperty] public int LevelStateId { get; set; }

		[BindProperty] public SaveName SaveName { get; set; } = default!;

		public async Task<ActionResult> OnPost(int? levelStateId)
		{
			if (ModelState.IsValid && levelStateId != null) {
				await SaveLevelState(RestoreGameStateFromDb((int) levelStateId), SaveName.Name);
			} else {
				return Page();
			}

			return RedirectToPage("./Play", new {levelStateId});
		}

		public ActionResult OnGet(int? levelStateId)
		{
			if (levelStateId == null) {
				return NotFound();
			}

			LevelStateId = (int) levelStateId;

			return Page();
		}
	}
}