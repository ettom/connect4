using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core;
using Scene = System.Collections.Generic.List<string>;


namespace GameEngine
{
	public static class Transform
	{
		private const string VerticalSeparator = "|";
		private const string HorizontalSeparator = "-";
		private const string EmptyCellPlaceHolder = "e";

		public static LevelState ParseScene(Scene scene, bool testing = false)
		{
			scene = RemoveDecorators(scene);

			var result = new LevelState(new Settings(testing));


			for (var row = 0; row < scene.Count; row++) {
				for (var column = 0; column < scene[row].Length; column++) {
					char c = scene[row][column];
					if (c == Convert.ToChar(EmptyCellPlaceHolder)) {
						continue;
					}

					var player = FindPlayerBySymbol(result, c);
					player.Discs.Add(new Disc(column, row));
				}
			}

			return result;
		}

		private static Player FindPlayerBySymbol(LevelState state, char symbol)
		{
			return state.Players.Single(player => player.Symbol == symbol);
		}

		public static Scene RemoveDecorators(IEnumerable<string> scene) =>
			(from row in scene
				where !row[0].Equals('-')
				let pattern = Regex.Escape(VerticalSeparator) + @"   "
				select Regex.Replace(row, pattern, EmptyCellPlaceHolder)
				into resultRow
				select resultRow.Replace(VerticalSeparator, "")
				into resultRow
				select resultRow.Replace(" ", "")).ToList();

		public static Scene RenderScene(LevelState level)
		{
			var result = new Scene();
			for (var i = 0; i < level.Height; i++) {
				result.Add(string.Concat(Enumerable.Repeat(" ", level.Width)));
			}

			PutDisks(level, ref result);
			PutDecorators(level, ref result);

			return result;
		}

		private static void PutDecorators(LevelState level, ref Scene scene)
		{
			var result = new Scene();
			for (var row = 0; row < level.Height; row++) {
				var line = new string("");
				for (var column = 0; column < level.Width; column++)
					line += VerticalSeparator + " " + scene[row][column] + " " + VerticalSeparator;

				result.Add(line);
				result.Add(string.Concat(Enumerable.Repeat(HorizontalSeparator, level.Width * 5)));
			}

			scene = result;
		}

		private static void PutDisks(LevelState level, ref Scene scene)
		{
			foreach (var player in level.Players) {
				foreach (var disc in player.Discs) {
					int x = disc.X;
					int y = disc.Y;
					var sb = new StringBuilder(scene[y]);
					sb.Remove(x, 1);

					sb.Insert(x, player.Symbol);
					scene[y] = sb.ToString();
				}
			}
		}
	}
}