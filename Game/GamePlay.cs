using System;
using System.Threading;
using Core;
using GameEngine;
using InputOutput;
using static GameEngine.Transform;
using static LoadSave.SaveGameHandler;
using static InputOutput.InputGetter;
using static InputOutput.Printer;
using Scene = System.Collections.Generic.List<string>;

namespace Game
{
	public static class GamePlay
	{
		public static void Run(Settings settings, LevelState? state = null)
		{
			if (state == null) {
				state = new LevelState(settings);
			}

			Play(state);
		}

		private static void Play(LevelState state)
		{
			int cursorPos = 1;
			while (state.GetWinnerElseNull() == null && !state.IsTie) {
				var scene = RenderScene(state);
				var currentPlayer = state.GetCurrentPlayer();
				PrintBoard(state, cursorPos, scene, currentPlayer);

				if (currentPlayer.isAI) {
					state = LevelUpdate.UpdateLevel(state, state.Turn);
					Thread.Sleep(500); // add delay for dramatic effect
					continue;
				}

				Key input;
				while ((input = GetUserDirectionInput()) != Key.Down) {
					switch (input) {
					case Key.Left:
						cursorPos = cursorPos <= 1 ? cursorPos : cursorPos - 1;
						break;
					case Key.Right:
						cursorPos = cursorPos >= state.Width ? cursorPos : cursorPos + 1;
						break;
					case Key.Save:
						Console.Clear();
						SaveGame(state);
						break;
					case Key.Exit:
						goto ExitLoop;
					}

					PrintBoard(state, cursorPos, scene, currentPlayer);
				}

				state = LevelUpdate.UpdateLevel(state, state.Turn, cursorPos);
			}

			ExitLoop:

			PrintBoard(state, cursorPos);
			PrintWinMessage(state);
		}

		private static void SaveGame(LevelState state)
		{
			var name = GetSaveNameFromUser(GetSaveGameNames());
			if (name != null) {
				SaveLevelState(state, name);
			}
		}
	}
}