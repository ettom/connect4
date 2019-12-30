namespace Core
{
	public class Disc
	{
		public int DiscId { get; set; }
		public int PlayerId { get; set; }
		public Player Player { get; set; } = default!;

		public int X { get; set; }
		public int Y { get; set; }


		public Disc()
		{
		}

		public Disc(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"x:{X}, y:{Y}";
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Disc p)) {
				return false;
			}

			return X == p.X && Y == p.Y;
		}

		public override int GetHashCode()
		{
			int tmp = X + (Y + 1) / 2;
			return X + tmp * tmp;
		}
	}
}