using Microsoft.AspNetCore.Mvc;
using static LoadSave.SettingsHandler;

namespace WebApp.Pages.Game.Settings
{
	public class AIPlayersModel : SettingsModel
	{
		private readonly string[] _propsToKeep = {"PlayerPrototypes"};

		public ActionResult OnPost()
		{
			IgnoreFields(ModelState, _propsToKeep);

			if (TryValidateModel(ModelState, nameof(ModelState))) {
				SaveSettings(TrackedSettings);
			} else {
				TrackedSettings = GetSavedSettingsElseNew();
				return Page();
			}

			return RedirectToPage("./Index");
		}
	}
}