using Core;
using FluentAssertions;
using NUnit.Framework;
using static GameEngine.LevelUpdate;
using static GameEngine.Transform;
using Scene = System.Collections.Generic.List<string>;

namespace Tests
{
	public class AITest
	{
		[Test]
		public void givenStateWhere1ColumnFree_callingUpdateLevel_mustReturnStateWhereAIDroppedToFreeColumn()
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
			LevelState result = UpdateLevel(inputState, 1);

			// ASSERT
			result.Should().BeEquivalentTo(expectedState);
			expectedState.IsTie.Should().BeTrue();
		}
	}
}