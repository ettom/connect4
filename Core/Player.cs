using System.Collections.Generic;

namespace Core
{
	public class Player
	{
		public int PlayerId { get; set; }

		public int LevelStateId { get; set; }

		public LevelState LevelState { get; set; } = default!;

		public HashSet<Disc> Discs { get; set; } = new HashSet<Disc>();
		public bool HasWon { get; set; }
		public char Symbol { get; set; }
		public int Number { get; set; }

		public bool isAI { get; set; }

		public Player()
		{
		}

		public Player(PlayerPrototype prototype)
		{
			Number = prototype.Number;
			Symbol = prototype.Symbol;
			isAI = prototype.isAI;
		}
	}
}