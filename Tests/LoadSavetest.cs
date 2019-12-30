using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using FluentAssertions;
using NUnit.Framework;
using static GameEngine.LevelUpdate;
using static GameEngine.Transform;
using static LoadSave.SaveGameHandler;
using static LoadSave.SettingsHandler;
using Scene = System.Collections.Generic.List<string>;

namespace Tests
{
	public class LoadSaveTest
	{
		[Test]
		public void givenState_callingSaveLevelState_mustReturnStateInDatabase()
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

			LevelState inputState = ParseScene(inputScene, true);

			var expectedPlayerOneDiscs = new List<Disc> {
				new Disc(0, 1),
				new Disc(0, 2),
				new Disc(2, 2)
			};

			var expectedPlayerTwoDiscs = new List<Disc> {
				new Disc(1, 2),
				new Disc(2, 1)
			};

			var saveGameName = Guid.NewGuid().ToString();

			// ACT
			SaveLevelState(inputState, saveGameName);
			LevelState result = LoadSaveGame(saveGameName);
			DeleteSaveGame(saveGameName);

			// ASSERT

			result.Width.Should().Be(3);
			result.Height.Should().Be(3);
			result.Players[0].Discs.Should().BeEquivalentTo(expectedPlayerOneDiscs);
			result.Players[1].Discs.Should().BeEquivalentTo(expectedPlayerTwoDiscs);
			result.Turn.Should().Be(1);
			result.IsTie.Should().BeFalse();
		}

		[Test]
		public void givenState_callingSaveLevelStateTwice_mustReturnStateInDatabase()
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

			LevelState inputState = ParseScene(inputScene, true);

			var expectedPlayerOneDiscs = new List<Disc> {
				new Disc(0, 1),
				new Disc(0, 2),
				new Disc(2, 2)
			};

			var expectedPlayerTwoDiscs = new List<Disc> {
				new Disc(1, 2),
				new Disc(2, 1)
			};

			var saveGameName1 = Guid.NewGuid().ToString();
			var saveGameName2 = Guid.NewGuid().ToString();

			// ACT
			SaveLevelState(inputState, saveGameName1);
			LevelState result1 = LoadSaveGame(saveGameName1);

			SaveLevelState(result1, saveGameName2);
			LevelState result2 = LoadSaveGame(saveGameName2);

			DeleteSaveGame(saveGameName1);
			DeleteSaveGame(saveGameName2);

			// ASSERT

			result2.Width.Should().Be(3);
			result2.Height.Should().Be(3);
			result2.Players.Single(p => p.Number == 1).Discs.Should().BeEquivalentTo(expectedPlayerOneDiscs);
			result2.Players.Single(p => p.Number == 2).Discs.Should().BeEquivalentTo(expectedPlayerTwoDiscs);
			result2.Turn.Should().Be(1);
			result2.IsTie.Should().BeFalse();
		}

		[Test]
		public void givenState_callingSaveLevelTwice_mustNotModifyPreviousSave()
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

			LevelState inputState = ParseScene(inputScene, true);

			var expectedPlayerOneDiscs = new List<Disc> {
				new Disc(0, 1),
				new Disc(0, 2),
				new Disc(2, 2)
			};

			var expectedPlayerTwoDiscs = new List<Disc> {
				new Disc(1, 2),
				new Disc(2, 1)
			};

			var saveGameName1 = Guid.NewGuid().ToString();
			var saveGameName2 = Guid.NewGuid().ToString();

			// ACT
			SaveLevelState(inputState, saveGameName1);
			LevelState result1 = LoadSaveGame(saveGameName1);


			SaveLevelState(result1, saveGameName2);
			LevelState result2 = LoadSaveGame(saveGameName2);

			// update second save
			UpdateLevel(result2, 1, 1);

			DeleteSaveGame(saveGameName1);
			DeleteSaveGame(saveGameName2);

			// ASSERT
			// first save must not change
			result1.Width.Should().Be(3);
			result1.Height.Should().Be(3);
			result1.Players[0].Discs.Should().BeEquivalentTo(expectedPlayerOneDiscs);
			result1.Players[1].Discs.Should().BeEquivalentTo(expectedPlayerTwoDiscs);
			result1.Turn.Should().Be(1);
			result1.IsTie.Should().BeFalse();
		}

		// This test deletes the current settings from the db and replaces them with defaults from Settings ctor
		[Test]
		public void givenSettings_callingSaveSettings_mustReturnStateInDatabase()
		{
			// ARRANGE
			var settings = new Settings {
				Turn = 1,
				Width = 3,
				Height = 4,
				StrikeSize = 3,
			};

			// ACT
			SaveSettings(settings);
			var loaded = LoadSettings();

			// replace settings with defaults
			SaveSettings(new Settings());

			// ASSERT
			// exclude id's
			settings.Should().BeEquivalentTo(loaded,
				options => options.Excluding(
					su => Regex.IsMatch(su.SelectedMemberPath, ".*Id")));
		}
	}
}