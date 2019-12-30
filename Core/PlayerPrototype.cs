namespace Core
{
	public class PlayerPrototype
	{
		public PlayerPrototype()
		{
		}

		public PlayerPrototype(int number)
		{
			Symbol = (char) (number + 48);
			Number = number;
		}


		public int PlayerPrototypeId { get; set; }
		public int SettingsId { get; set; }

		public int Number { get; set; }
		public char Symbol { get; set; }
		public bool isAI { get; set; }
	}
}