using System;
using System.Linq;
using Core;
using static GameEngine.Transform;
using Scene = System.Collections.Generic.List<string>;

namespace InputOutput
{
	public static class Printer
	{
		public static void PrintBoard(LevelState levelState, int pos, Scene? scene = null, Player? current = null)
		{
			PrintHeader(current ?? levelState.Players.Single(p => p.Number == levelState.Turn),
				levelState.Width, pos);
			PrintScene(scene ?? RenderScene(levelState));
		}

		private static void PrintScene(Scene scene)
		{
			foreach (var line in scene) {
				Console.Out.WriteLine(line);
			}
		}

		public static string GetWinMessage(LevelState state)
		{
			if (state.IsTie) {
				return "It's a tie!";
			}

			var winner = state.Players.FirstOrDefault(player => player.HasWon);
			return winner != null ? $"Player {winner.Number} ({winner.Symbol}) won!" : "";
		}

		public static void PrintWinMessage(LevelState state)
		{
			PrintMessage(GetWinMessage(state), false);
		}


		private static void PrintHeader(Player player, int width, int cursorPos)
		{
			Console.Clear();
			Console.Out.WriteLine("Press S to save, Q to quit.");
			Console.Out.Write($"Player {player.Number} moves.");
			Console.Out.WriteLine();

			Console.Out.Write("  ");
			for (int i = 1; i < width + 1; i++) {
				if (i == cursorPos) {
					Console.Out.Write(player.Symbol);
				} else {
					Console.Out.Write(" ");
				}

				Console.Out.Write("    ");
			}

			Console.Out.WriteLine();
		}

		public static void PrintMessage(string message, bool toClear = true)
		{
			if (toClear) {
				Console.Clear();
			}

			Console.Out.WriteLine(message);
			Console.Out.WriteLine("Press any key to continue.");
			Console.ReadKey();
		}
	}
}