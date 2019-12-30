using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LoadSave.SettingsHandler;

namespace WebApp.Pages.Game.Settings
{
	public abstract class SettingsModel : PageModel
	{
		[BindProperty] public Core.Settings TrackedSettings { get; set; } = GetSavedSettingsElseNew();
		private Core.Settings UnTrackedSettings { get; set; } = GetSavedSettingsElseNew();

		private static readonly string[] SettingsProperties =
			{"Width", "Height", "StrikeSize", "PlayerPrototypes", "Turn"};

		protected void IgnoreFields(ModelStateDictionary dictionary, string[] propsToKeep)
		{
			foreach (var prop in SettingsProperties) {
				if (propsToKeep.Contains(prop)) {
					continue;
				}

				// set all props that are not marked for keeping to values from UnTrackedSettings
				var trackedProp = TrackedSettings.GetType().GetProperty(prop);
				var unTrackedProp = UnTrackedSettings.GetType().GetProperty(prop);
				trackedProp?.SetValue(TrackedSettings, unTrackedProp?.GetValue(UnTrackedSettings), null);

				// remove key from model so it doesn't get validated
				dictionary.Remove($"TrackedSettings.{prop}");
			}
		}

		public void OnGet()
		{
			TrackedSettings = GetSavedSettingsElseNew();
			UnTrackedSettings = GetSavedSettingsElseNew();
		}
	}
}