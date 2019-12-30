using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static LoadSave.SettingsHandler;

namespace WebApp.Pages.Game.Settings
{
	public class BoardDimensionsModel : SettingsModel
	{
		private readonly string[] _propsToKeep = {"Width", "Height", "StrikeSize"};

		public async Task<ActionResult> OnPost()
		{
			IgnoreFields(ModelState, _propsToKeep);

			if (TryValidateModel(ModelState, nameof(ModelState))) {
				await SaveSettings(TrackedSettings);
			} else {
				TrackedSettings = GetSavedSettingsElseNew();
				return Page();
			}

			return RedirectToPage("./Index");
		}
	}
}