using System.Collections.Generic;
using System.Linq;


namespace Core
{
	public class LevelState
	{
		public int LevelStateId { get; set; }

		public int? SaveGameId { get; set; }
		public SaveGame? SaveGame { get; set; }

		public List<Player> Players { get; set; } = new List<Player>();


		public int Width { get; set; }
		public int Height { get; set; }

		public int Turn { get; set; }
		public bool IsTie { get; set; }

		public int StrikeSize { get; set; }

		public LevelState()
		{
		}

		public LevelState(Settings settings)
		{
			Turn = settings.Turn;
			Height = settings.Height;
			Width = settings.Width;
			StrikeSize = settings.StrikeSize;

			foreach (var proto in settings.PlayerPrototypes) {
				Players.Add(new Player(proto));
			}
		}

		public Player GetCurrentPlayer()
		{
			return Players.Single(p => p.Number == Turn);
		}

		public Player GetWinnerElseNull()
		{
			return Players.FirstOrDefault(player => player.HasWon);
		}

		public bool IsGameOver()
		{
			return IsTie || Players.Any(player => player.HasWon);
		}
	}
}