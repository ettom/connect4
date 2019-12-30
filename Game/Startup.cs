using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using Core;
using LoadSave;
using MenuSystem;
using static Core.Validators;
using static Game.GamePlay;
using static InputOutput.InputGetter;
using static InputOutput.Printer;
using static LoadSave.SettingsHandler;

namespace Game
{
	public static class Startup
	{
		private static LevelState SavedState { get; set; } = default!;
		private static Settings Settings { get; set; } = GetSavedSettingsElseNew();

		private static readonly Menu SettingsMenu = new Menu(1) {
			Title = "Settings",
			MenuItemsDictionary = new Dictionary<string, MenuItem> {
				{
					"1", new MenuItem {
						Title = "Set board dimensions",
						CommandToExecute = SetBoardDimensions
					}
				}, {
					"2", new MenuItem {
						Title = "Set strike size",
						CommandToExecute = SetStrikeSize
					}
				}, {
					"3", new MenuItem {
						Title = "Choose player symbols",
						CommandToExecute = ChoosePlayerSymbols
					}
				}, {
					"4", new MenuItem {
						Title = "Choose which players are controlled by AI",
						CommandToExecute = ChooseAiPlayers
					}
				}, {
					"5", new MenuItem {
						Title = "Reset to default settings",
						CommandToExecute = ResetSettings
					}
				}
			}
		};


		private static readonly Menu LaunchMenu = new Menu(1) {
			Title = "Start a new game",
			MenuItemsDictionary = new Dictionary<string, MenuItem> {
				{
					"1", new MenuItem {
						Title = "Start a new game",
						CommandToExecute = StartNewGame
					}
				}
			}
		};

		private static readonly Menu LoadSaveMenu = new Menu(1) {
			Title = "Load/save settings or games",
			MenuItemsDictionary = new Dictionary<string, MenuItem> {
				{
					"1", new MenuItem {
						Title = "Load saved game",
						CommandToExecute = LoadSaveGame
					}
				}
			}
		};

		private static readonly Menu MainMenu = new Menu {
			Title = "Main menu",
			MenuItemsDictionary = new Dictionary<string, MenuItem> {
				{
					"R", new MenuItem {
						Title = "Run game",
						CommandToExecute = LaunchMenu.Run
					}
				}, {
					"S", new MenuItem {
						Title = "Settings",
						CommandToExecute = SettingsMenu.Run
					}
				}, {
					"L", new MenuItem {
						Title = "Load a saved game",
						CommandToExecute = LoadSaveMenu.Run
					}
				}
			}
		};

		private static Menu _loadSaveGamesMenu = new Menu(2) {
			Title = "Load/save games",
			MenuItemsDictionary = new Dictionary<string, MenuItem>()
		};

		private static string? ResetSettings()
		{
			Settings = new Settings(false);
			SaveSettings(Settings);

			Console.Clear();
			Console.Out.WriteLine("Settings reset to default!");
			Thread.Sleep(700);
			Console.Clear();
			return null;
		}


		private static string? LoadSaveGame()
		{
			var saveGames = SaveGameHandler.GetSaveGameNames();
			if (saveGames.Count == 0) {
				PrintMessage("No saved games found!");
				return null;
			}

			int count = 1;
			foreach (var tmp in from name in saveGames
				where name != null
				select new MenuItem {
					Title = name,
					CommandToExecute = () => {
						SetSaveGame(name);
						Run(Settings, SavedState);
						return null;
					}
				}) {
				_loadSaveGamesMenu.MenuItemsDictionary.Add((count++).ToString(), tmp);
			}

			_loadSaveGamesMenu.Run();
			_loadSaveGamesMenu = new Menu(2) {
				Title = "Load/save games",
				MenuItemsDictionary = new Dictionary<string, MenuItem>()
			};
			return null;
		}

		private static void SetSaveGame(string name)
		{
			SavedState = SaveGameHandler.LoadSaveGame(name);
		}

		private static string? ChoosePlayerSymbols()
		{
			Console.Clear();

			var validationErrors = new List<ValidationResult>();
			foreach (var player in Settings.PlayerPrototypes) {
				var previousSymbol = player.Symbol;
				do {
					player.Symbol = previousSymbol;
					var currentSymbolString = $"\nCurrent symbol is {player.Symbol}";

					if (validationErrors.Count > 0) {
						currentSymbolString = CreateErrorString(validationErrors) + currentSymbolString;
					}

					var input = GetUserStringInput(
						$"Enter symbol for player number {player.Number}:\n{currentSymbolString}",
						1, 1, "");

					Console.Clear();

					if (input.wasCanceled) {
						break;
					}

					player.Symbol = Convert.ToChar(input.result);
					validationErrors = ValidateProperty(Settings, "PlayerPrototypes");
				} while (validationErrors.Count > 0);
			}


			SaveSettings(Settings);
			return null;
		}

		private static string? SetBoardDimensions()
		{
			GetIntInputWithValidator(Settings, "Width");
			GetIntInputWithValidator(Settings, "Height");

			Settings.StrikeSize = Math.Max(Settings.Height, Settings.Width);
			SaveSettings(Settings);
			return null;
		}

		private static string? SetStrikeSize()
		{
			GetIntInputWithValidator(Settings, "StrikeSize");

			SaveSettings(Settings);
			return null;
		}

		public static void RunMenus()
		{
			Console.Clear();
			MainMenu.Run();
		}

		private static string? ChooseAiPlayers()
		{
			string DrawAiPlayers()
			{
				var sb = new StringBuilder();

				foreach (var player in Settings.PlayerPrototypes) {
					sb.Append($"\nPlayer {player.Number}: {player.isAI}");
				}

				return sb.ToString();
			}

			Console.Clear();
			do {
				var input = GetUserIntInput(
					"Enter a player number to toggle if it is controlled by AI:" + DrawAiPlayers(),
					1, Settings.PlayerPrototypes.Count, "Q");

				if (input.wasCanceled) {
					break;
				}

				Settings.PlayerPrototypes.Single(p => p.Number == input.result).isAI ^= true;

				var validationErrors = ValidateProperty(Settings, "PlayerPrototypes");
				if (validationErrors.Count > 0) {
					Settings.PlayerPrototypes.Single(p => p.Number == input.result).isAI = false;
					Console.Clear();
					Console.Out.WriteLine(CreateErrorString(validationErrors));
					continue;
				}

				Console.Clear();
			} while (true);

			SaveSettings(Settings);
			return null;
		}

		private static string? StartNewGame()
		{
			Run(Settings);
			return null;
		}
	}
}