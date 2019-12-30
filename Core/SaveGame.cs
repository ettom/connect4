using System.ComponentModel.DataAnnotations;

namespace Core
{
	public class SaveGame
	{
		public int SaveGameId { get; set; }

		public LevelState LevelState { get; set; } = default!;


		[MinLength(1)] [MaxLength(32)] public string Name { get; set; } = default!;
	}
}