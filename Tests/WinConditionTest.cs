using Core;
using FluentAssertions;
using GameEngine;
using NUnit.Framework;
using static GameEngine.Transform;
using Scene = System.Collections.Generic.List<string>;

namespace Tests
{
	public class WinConditionTest
	{
		[Test]
		public void givenInputWhereNobodyWins_callingCheckWin_mustReturnFalse()
		{
			// ARRANGE
			const int strikeSize = 3;
			var inputScene = new Scene(new[] {
				"| 2 || 1 || 1 |",
				"---------------",
				"| 1 || 2 || 2 |",
				"---------------",
				"| 2 || 1 || 1 |",
				"---------------"
			});

			var lastDisc = new Disc(1, 0);
			var state = ParseScene(inputScene, true);
			state.StrikeSize = strikeSize;
			state.Turn = 1;

			// ACT
			bool horizontalResult = LevelUpdate.CheckHorizontal(state, lastDisc);
			bool verticalResult = LevelUpdate.CheckVertical(state, lastDisc);
			bool forwardSlashResult = LevelUpdate.CheckForwardSlash(state, lastDisc);
			bool backwardSlashResult = LevelUpdate.CheckBackwardSlash(state, lastDisc);

			// ASSERT
			horizontalResult.Should().BeFalse();
			verticalResult.Should().BeFalse();
			forwardSlashResult.Should().BeFalse();
			backwardSlashResult.Should().BeFalse();
		}

		[Test]
		public void givenInputWherePlayer2WinsByBackwardSlash1_callingCheckWin_mustReturnTrue()
		{
			// ARRANGE
			const int strikeSize = 4;
			var inputScene = new Scene(new[] {
				"| 2 ||   ||   ||   |",
				"--------------------",
				"| 1 || 2 ||   ||   |",
				"--------------------",
				"| 2 || 1 || 2 ||   |",
				"--------------------",
				"| 1 || 1 || 1 || 2 |",
				"--------------------"
			});

			var lastDisc = new Disc(3, 3);
			var state = ParseScene(inputScene, true);
			state.StrikeSize = strikeSize;
			state.Turn = 2;

			// ACT
			bool horizontalResult = LevelUpdate.CheckHorizontal(state, lastDisc);
			bool verticalResult = LevelUpdate.CheckVertical(state, lastDisc);
			bool forwardSlashResult = LevelUpdate.CheckForwardSlash(state, lastDisc);
			bool backwardSlashResult = LevelUpdate.CheckBackwardSlash(state, lastDisc);

			// ASSERT
			horizontalResult.Should().BeFalse();
			verticalResult.Should().BeFalse();
			forwardSlashResult.Should().BeFalse();
			backwardSlashResult.Should().BeTrue();
		}

		[Test]
		public void givenInputWherePlayer2WinsByBackwardSlash2_callingCheckWin_mustReturnTrue()
		{
			// ARRANGE
			const int strikeSize = 4;
			var inputScene = new Scene(new[] {
				"| 2 ||   ||   ||   |",
				"--------------------",
				"| 1 || 2 ||   ||   |",
				"--------------------",
				"| 2 || 1 || 2 ||   |",
				"--------------------",
				"| 1 || 1 || 1 || 2 |",
				"--------------------"
			});

			var lastDisc = new Disc(0, 0);
			var state = ParseScene(inputScene, true);
			state.StrikeSize = strikeSize;
			state.Turn = 2;

			// ACT
			bool horizontalResult = LevelUpdate.CheckHorizontal(state, lastDisc);
			bool verticalResult = LevelUpdate.CheckVertical(state, lastDisc);
			bool forwardSlashResult = LevelUpdate.CheckForwardSlash(state, lastDisc);
			bool backwardSlashResult = LevelUpdate.CheckBackwardSlash(state, lastDisc);

			// ASSERT
			horizontalResult.Should().BeFalse();
			verticalResult.Should().BeFalse();
			forwardSlashResult.Should().BeFalse();
			backwardSlashResult.Should().BeTrue();
		}


		[Test]
		public void givenInputWherePlayer2WinsByForwardSlash1_callingCheckWin_mustReturnTrue()
		{
			// ARRANGE
			const int strikeSize = 4;
			var inputScene = new Scene(new[] {
				"|   ||   ||   || 2 |",
				"--------------------",
				"|   ||   || 2 || 1 |",
				"--------------------",
				"|   || 2 || 1 || 1 |",
				"--------------------",
				"| 2 || 1 || 1 || 2 |",
				"--------------------"
			});

			var lastDisc = new Disc(3, 0);
			var state = ParseScene(inputScene, true);
			state.StrikeSize = strikeSize;
			state.Turn = 2;

			// ACT
			bool horizontalResult = LevelUpdate.CheckHorizontal(state, lastDisc);
			bool verticalResult = LevelUpdate.CheckVertical(state, lastDisc);
			bool forwardSlashResult = LevelUpdate.CheckForwardSlash(state, lastDisc);
			bool backwardSlashResult = LevelUpdate.CheckBackwardSlash(state, lastDisc);

			// ASSERT
			horizontalResult.Should().BeFalse();
			verticalResult.Should().BeFalse();
			forwardSlashResult.Should().BeTrue();
			backwardSlashResult.Should().BeFalse();
		}

		[Test]
		public void givenInputWherePlayer2WinsByForwardSlash2_callingCheckWin_mustReturnTrue()
		{
			// ARRANGE
			const int strikeSize = 4;
			var inputScene = new Scene(new[] {
				"|   ||   ||   || 2 |",
				"--------------------",
				"|   ||   || 2 || 1 |",
				"--------------------",
				"|   || 2 || 1 || 1 |",
				"--------------------",
				"| 2 || 1 || 1 || 2 |",
				"--------------------"
			});

			var lastDisc = new Disc(2, 1);
			var state = ParseScene(inputScene, true);
			state.StrikeSize = strikeSize;
			state.Turn = 2;

			// ACT
			bool horizontalResult = LevelUpdate.CheckHorizontal(state, lastDisc);
			bool verticalResult = LevelUpdate.CheckVertical(state, lastDisc);
			bool forwardSlashResult = LevelUpdate.CheckForwardSlash(state, lastDisc);
			bool backwardSlashResult = LevelUpdate.CheckBackwardSlash(state, lastDisc);

			// ASSERT
			horizontalResult.Should().BeFalse();
			verticalResult.Should().BeFalse();
			forwardSlashResult.Should().BeTrue();
			backwardSlashResult.Should().BeFalse();
		}
	}
}