using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace GameEngine
{
	public static class LevelUpdate
	{
		private static List<Disc> GetAllDiscsInColumn(LevelState state, int column)
		{
			return (from player in state.Players
				from disc in player.Discs
				where disc.X == column
				select disc).ToList();
		}

		private static int GetFirstFreePositionInColumn(LevelState state, int column)
		{
			var discs = GetAllDiscsInColumn(state, column);
			return state.Height - discs.Count - 1;
		}

		private static List<int> GetAllFreeColumns(LevelState state)
		{
			var freeColumns = new List<int>();
			for (var i = 0; i < state.Width; i++) {
				if (GetFirstFreePositionInColumn(state, i) != -1) {
					freeColumns.Add(i);
				}
			}

			return freeColumns;
		}


		public static LevelState UpdateLevel(LevelState state, int playerNumber, int? input = null)
		{
			state.Turn = playerNumber;
			int x, y;
			if (input == null) {
				(x, y) = GetAIMoveCoordinates(state);
			} else {
				x = (int) input - 1;
				y = GetFirstFreePositionInColumn(state, x);
			}

			if (y == -1) {
				return state;
			}

			var discToPut = new Disc(x, y);

			var player = state.GetCurrentPlayer();
			player.Discs.Add(discToPut);
			player.HasWon = CheckWin(state, discToPut);
			state.IsTie = CheckTie(state);

			IncrementTurn(state);
			return state;
		}

		private static void IncrementTurn(LevelState state)
		{
			state.Turn = (state.Turn + 1) % (state.Players.Count + 1);
			state.Turn = Math.Max(state.Turn, 1);
		}

		private static (int x, int y) GetAIMoveCoordinates(LevelState state)
		{
			var freeColumns = GetAllFreeColumns(state);
			var maxValue = freeColumns.Count - 1;
			var x = freeColumns[new Random().Next(maxValue)];
			var y = GetFirstFreePositionInColumn(state, x);
			return (x, y);
		}

		private static bool CheckTie(LevelState state)
		{
			return state.Players.Sum(player => player.Discs.Count) == state.Width * state.Height;
		}

		private static bool CheckWin(LevelState state, Disc lastDisc)
		{
			return CheckHorizontal(state, lastDisc)
			       || CheckVertical(state, lastDisc)
			       || CheckForwardSlash(state, lastDisc)
			       || CheckBackwardSlash(state, lastDisc);
		}


		public static bool CheckHorizontal(LevelState state, Disc lastDisc)
		{
			Player player = state.GetCurrentPlayer();
			int count = 1;
			int y = lastDisc.Y;

			for (int i = lastDisc.X + 1; i <= state.Width; i++) {
				var rightDisc = new Disc(i, y);
				if (player.Discs.Contains(rightDisc)) {
					++count;
				} else {
					break;
				}
			}

			for (int i = lastDisc.X - 1; i >= 0; i--) {
				var leftDisc = new Disc(i, y);
				if (player.Discs.Contains(leftDisc)) {
					++count;
				} else {
					break;
				}
			}

			return count == state.StrikeSize;
		}


		public static bool CheckVertical(LevelState state, Disc lastDisc)
		{
			Player player = state.GetCurrentPlayer();
			int count = 1;
			int x = lastDisc.X;

			for (int i = lastDisc.Y + 1; i <= state.Height; i++) {
				var lowerDisc = new Disc(x, i);
				if (player.Discs.Contains(lowerDisc)) {
					++count;
				} else {
					break;
				}
			}

			return count == state.StrikeSize;
		}

		public static bool CheckForwardSlash(LevelState state, Disc lastDisc)
		{
			Player player = state.GetCurrentPlayer();
			int count = 1;
			int x = lastDisc.X;
			int y = lastDisc.Y;

			for (int i = x - 1, j = y + 1; i >= 0 && j >= 0; i--, j++) {
				var lowerLeftDisc = new Disc(i, j);
				if (player.Discs.Contains(lowerLeftDisc)) {
					++count;
				} else {
					break;
				}
			}

			for (int i = x + 1, j = y - 1; i <= state.Width && j <= state.Height; i++, j--) {
				var upperRightDisc = new Disc(i, j);
				if (player.Discs.Contains(upperRightDisc)) {
					++count;
				} else {
					break;
				}
			}

			return count == state.StrikeSize;
		}

		public static bool CheckBackwardSlash(LevelState state, Disc lastDisc)
		{
			Player player = state.GetCurrentPlayer();
			int count = 1;
			int y = lastDisc.Y;
			int x = lastDisc.X;


			for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--) {
				var upperLeftDisc = new Disc(i, j);
				if (player.Discs.Contains(upperLeftDisc)) {
					++count;
				} else {
					break;
				}
			}

			for (int i = x + 1, j = y + 1; i <= state.Width && j <= state.Height; i++, j++) {
				var lowerRightDisc = new Disc(i, j);
				if (player.Discs.Contains(lowerRightDisc)) {
					++count;
				} else {
					break;
				}
			}

			return count == state.StrikeSize;
		}
	}
}