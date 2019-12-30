using Core;
using FluentAssertions;
using NUnit.Framework;
using static GameEngine.LevelUpdate;
using static GameEngine.Transform;
using Scene = System.Collections.Generic.List<string>;

namespace Tests
{
	public class GameLogicTest
	{
		[Test]
		public void givenStateWherePlayer1Moves_callingUpdateLevel_mustReturnStateWhereItsPlayer2sTurn()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);
			const int columnToDrop = 2;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Turn.Should().Be(2);
		}

		[Test]
		public void givenStateWherePlayer2Moves_callingUpdateLevel_mustReturnStateWhereItsPlayer1sTurn()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);
			const int columnToDrop = 2;

			// ACT
			LevelState result = UpdateLevel(inputState, 2, columnToDrop);

			// ASSERT
			result.Turn.Should().Be(1);
		}

		[Test]
		public void givenInputWherePlayerDropsToColumn_callingUpdateLevel_mustReturnStateWhereDiskOnBottomOfColumn()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);

			const int columnToDrop = 2;
			var expectedScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"|   ||   ||   |",
				"---------------",
				"|   || 1 ||   |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.Turn = 2;


			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}


		[Test]
		public void givenInputWherePlayer1DropsToFullColumn_callingUpdateLevel_mustReturnUnchangedState()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   || 1 ||   |",
				"---------------",
				"|   || 2 ||   |",
				"---------------",
				"|   || 1 ||   |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);

			const int columnToDrop = 2;
			var expectedScene = new Scene(new[] {
				"|   || 1 ||   |",
				"---------------",
				"|   || 2 ||   |",
				"---------------",
				"|   || 1 ||   |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);


			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}

		[Test]
		public void givenInputWhereBoardIsFilled_callingUpdateLevel_mustReturnStateWhereItsATie()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"| 2 ||   || 1 |",
				"---------------",
				"| 1 || 2 || 2 |",
				"---------------",
				"| 2 || 1 || 1 |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);

			const int columnToDrop = 2;
			var expectedScene = new Scene(new[] {
				"| 2 || 1 || 1 |",
				"---------------",
				"| 1 || 2 || 2 |",
				"---------------",
				"| 2 || 1 || 1 |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.Turn = 2;
			expectedState.IsTie = true;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
			expectedState.IsTie.Should().BeTrue();
		}

		[Test]
		public void givenInputWherePlayer1WinsByHorizontal_callingUpdateLevel_mustReturnStateWherePlayer1Won()
		{
			// ARRANGE
			const int strikeSize = 3;
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 2 || 2 ||   |",
				"---------------",
				"| 1 || 1 ||   |",
				"---------------"
			});
			var inputState = ParseScene(inputScene, true);
			inputState.StrikeSize = strikeSize;

			const int columnToDrop = 3;
			var expectedScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 2 || 2 ||   |",
				"---------------",
				"| 1 || 1 || 1 |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.StrikeSize = strikeSize;
			expectedState.Turn = 2;
			expectedState.Players[0].HasWon = true;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}

		[Test]
		public void givenInputWherePlayer1WinsByVertical_callingUpdateLevel_mustReturnStateWherePlayer1Won()
		{
			// ARRANGE
			const int strikeSize = 3;
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------"
			});

			var inputState = ParseScene(inputScene, true);
			inputState.StrikeSize = strikeSize;

			const int columnToDrop = 2;
			var expectedScene = new Scene(new[] {
				"|   || 1 ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.StrikeSize = strikeSize;
			expectedState.Turn = 2;
			expectedState.Players[0].HasWon = true;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}

		[Test]
		public void givenInputWherePlayer1WinsByForwardSlash_callingUpdateLevel_mustReturnStateWherePlayer1Won()
		{
			// ARRANGE
			const int strikeSize = 3;
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"|   || 1 || 2 |",
				"---------------",
				"| 1 || 2 || 2 |",
				"---------------"
			});

			var inputState = ParseScene(inputScene, true);
			inputState.StrikeSize = strikeSize;

			const int columnToDrop = 3;
			var expectedScene = new Scene(new[] {
				"|   ||   || 1 |",
				"---------------",
				"|   || 1 || 2 |",
				"---------------",
				"| 1 || 2 || 2 |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.StrikeSize = strikeSize;
			expectedState.Turn = 2;
			expectedState.Players[0].HasWon = true;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}

		[Test]
		public void givenInputWherePlayer1WinsByBackwardSlash_callingUpdateLevel_mustReturnStateWherePlayer1Won()
		{
			// ARRANGE
			const int strikeSize = 3;
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------",
				"| 2 || 2 || 1 |",
				"---------------"
			});

			var inputState = ParseScene(inputScene, true);
			inputState.StrikeSize = strikeSize;

			const int columnToDrop = 1;
			var expectedScene = new Scene(new[] {
				"| 1 ||   ||   |",
				"---------------",
				"| 2 || 1 ||   |",
				"---------------",
				"| 2 || 2 || 1 |",
				"---------------"
			});
			var expectedState = ParseScene(expectedScene, true);
			expectedState.StrikeSize = strikeSize;
			expectedState.Turn = 2;
			expectedState.Players[0].HasWon = true;

			// ACT
			LevelState result = UpdateLevel(inputState, 1, columnToDrop);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
		}
	}
}