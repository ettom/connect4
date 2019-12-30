using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Core.Validators;

namespace Core
{
	public class Settings
	{
		public int SettingsId { get; set; }


		public Settings()
		{
		}

		public Settings(bool testing)
		{
			Turn = 1;
			if (testing) {
				Width = 3;
				Height = 3;
				StrikeSize = 3;

				PlayerPrototypes.Add(new PlayerPrototype(1));
				PlayerPrototypes.Add(new PlayerPrototype(2));
			} else {
				Width = 6;
				Height = 7;
				StrikeSize = 4;

				PlayerPrototypes.Add(new PlayerPrototype(1) {Symbol = '○'});
				PlayerPrototypes.Add(new PlayerPrototype(2) {Symbol = '●'});
			}
		}

		[AIPlayersValidator]
		[PlayerSymbolsValidator]
		public List<PlayerPrototype> PlayerPrototypes { get; set; } = new List<PlayerPrototype>();

		[StrikeSizeValidator("Width", "Height")]
		public int StrikeSize { get; set; }

		[Range(3, 10)] public int Width { get; set; }

		[Range(3, 10)] public int Height { get; set; }

		public int Turn { get; set; }
	}
}