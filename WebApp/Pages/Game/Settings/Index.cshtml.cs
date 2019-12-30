using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static LoadSave.SettingsHandler;

namespace WebApp.Pages.Game.Settings
{
	public class IndexModel : PageModel
	{
		public async Task OnGet(bool reset = false)
		{
			if (!reset) return;

			var settings = new Core.Settings(false);
			await SaveSettings(settings);
		}
	}
}