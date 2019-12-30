using System.Collections.Generic;
using Core;
using FluentAssertions;
using NUnit.Framework;
using static GameEngine.Transform;
using Scene = System.Collections.Generic.List<string>;


namespace Tests
{
	public class TransformTest
	{
		[Test]
		public void givenScene_callingParseScene_mustReturnLevelState()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 1 ||   || 2 |",
				"---------------",
				"| 1 || 2 || 1 |",
				"---------------"
			});

			var expectedPlayerOneDiscs = new List<Disc> {
				new Disc(0, 1),
				new Disc(0, 2),
				new Disc(2, 2)
			};

			var expectedPlayerTwoDiscs = new List<Disc> {
				new Disc(1, 2),
				new Disc(2, 1)
			};

			// ACT
			LevelState result = ParseScene(inputScene, true);

			// ASSERT
			result.Width.Should().Be(3);
			result.Height.Should().Be(3);
			result.Players[0].Discs.Should().BeEquivalentTo(expectedPlayerOneDiscs);
			result.Players[1].Discs.Should().BeEquivalentTo(expectedPlayerTwoDiscs);
		}


		[Test]
		public void givenLevel_callingRenderScene_mustReturnScene()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 1 ||   || 2 |",
				"---------------",
				"| 1 || 2 || 1 |",
				"---------------"
			});

			var expectedScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 1 ||   || 2 |",
				"---------------",
				"| 1 || 2 || 1 |",
				"---------------"
			});

			LevelState inputLevel = ParseScene(inputScene, true);

			// ACT
			Scene result = RenderScene(inputLevel);

			// ASSERT
			expectedScene.Should().BeEquivalentTo(result);
		}

		[Test]
		public void givenScene_callingRemoveDecorators_mustReturnSceneWithoutDecorators()
		{
			// ARRANGE
			var inputScene = new Scene(new[] {
				"|   ||   ||   |",
				"---------------",
				"| 1 ||   || 2 |",
				"---------------",
				"| 1 || 2 || 1 |",
				"---------------"
			});

			var expectedScene = new Scene(new[] {
				"eee",
				"1e2",
				"121"
			});

			// ACT
			var result = RemoveDecorators(inputScene);

			// ASSERT
			result.Should().BeEquivalentTo(expectedScene);
		}
	}
}