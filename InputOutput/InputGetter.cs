using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Text.RegularExpressions.Regex;
using static System.Text.RegularExpressions.RegexOptions;
using static Core.Validators;

namespace InputOutput
{
	public static class InputGetter
	{
		private static string SplitCamelCase(string input)
		{
			return Replace(input, "([A-Z])", " $1", Compiled).Trim().ToLower();
		}


		public static void GetIntInputWithValidator<T>(T parentObject, string propName,
			string cancelStrValue = "Q", string? propNameForDisplay = null) where T : notnull
		{
			var propNameForPrompt = propNameForDisplay ?? SplitCamelCase(propName);

			var prop = parentObject.GetType().GetProperty(propName);
			if (prop == null) throw new Exception("Property not found!");

			var previousVal = prop.GetValue(parentObject);
			var validationErrors = new List<ValidationResult>();
			string? consoleLine = "";
			var wasNotInt = false;

			do {
				Console.Clear();
				prop.SetValue(parentObject, previousVal, null);

				var currentValueString = $"Current {propNameForPrompt} is: {previousVal}";

				if (validationErrors.Count > 0) {
					currentValueString = CreateErrorString(validationErrors) + currentValueString;
				} else if (wasNotInt) {
					currentValueString = $"'{consoleLine}' cant be converted to int value!\n" + currentValueString;
					wasNotInt = false;
				}

				PrintPrompt(cancelStrValue, currentValueString, propNameForPrompt);

				consoleLine = Console.ReadLine()?.Trim();
				if (consoleLine?.ToUpper() == cancelStrValue) return;

				if (uint.TryParse(consoleLine, out var userInt)) {
					previousVal = prop.GetValue(parentObject);
					prop.SetValue(parentObject, Convert.ToInt32(userInt), null);
					validationErrors = ValidateProperty(parentObject, propName);
				} else {
					wasNotInt = true;
					validationErrors.Clear();
					Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
				}
			} while (validationErrors.Count > 0 || wasNotInt);
		}

		private static void PrintPrompt(string cancelStrValue, string prompt,
			string? propNameForPrompt = null)
		{
			Console.WriteLine(prompt);

			if (propNameForPrompt != null) {
				Console.WriteLine($"Enter {propNameForPrompt}:\n");
			}

			Console.WriteLine(cancelStrValue.Trim().Length == 0
				? "To cancel input, press enter."
				: $"To cancel input enter: {cancelStrValue}");

			Console.Write("> ");
		}


		public static (int result, bool wasCanceled) GetUserIntInput(string prompt, int min, int max,
			string cancelStrValue = "Q")
		{
			do {
				PrintPrompt(cancelStrValue, prompt);
				var consoleLine = Console.ReadLine()?.Trim();
				if (consoleLine?.ToUpper() == cancelStrValue) {
					return (0, true);
				}

				if (int.TryParse(consoleLine, out var userInt)) {
					var result = userInt;
					if (userInt < min) {
						Console.Clear();
						Console.Out.WriteLine($"{userInt} is less than minimum allowed value of {min}");
					} else if (userInt > max) {
						Console.Clear();
						Console.Out.WriteLine($"{userInt} is more than maximum allowed value of {max}");
					} else {
						return (result, false);
					}
				} else {
					Console.Clear();
					Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
				}
			} while (true);
		}

		public static (string result, bool wasCanceled) GetUserStringInput(string prompt, int min, int max,
			string cancelStrValue)
		{
			do {
				Console.WriteLine(prompt);

				Console.WriteLine(cancelStrValue.Trim().Length == 0
					? "To cancel input, press enter."
					: $"To cancel input enter: {cancelStrValue}");

				Console.Write("> ");

				var consoleLine = Console.ReadLine()?.Trim();
				if (consoleLine == null || consoleLine.ToUpper() == cancelStrValue) {
					return (string.Empty, true);
				}

				if (consoleLine.Length < min) {
					Console.Clear();
					Console.Out.WriteLine($"{consoleLine} is shorter than minimum allowed length of {min}");
				} else if (consoleLine.Length > max) {
					Console.Clear();
					Console.Out.WriteLine($"{consoleLine} is longer than maximum allowed length of {max}");
				} else {
					return (consoleLine, false);
				}
			} while (true);
		}

		private static bool GetUserYnInput(string prompt)
		{
			ConsoleKey input;
			do {
				Console.WriteLine(prompt + " [y/n]");
				Console.Write("> ");
				input = Console.ReadKey(false).Key;
				Console.Clear();
			} while (input != ConsoleKey.Y && input != ConsoleKey.N);

			return input == ConsoleKey.Y;
		}

		public static Key GetUserDirectionInput()
		{
			var input = Console.ReadKey(false).Key;
			Console.Write("\b \b"); // clear the last character
			return ConvertInputToDirection(input);
		}


		public static string? GetSaveNameFromUser(List<string> existingSaveGames)
		{
			string? fileName = null;
			while (true) {
				var input = GetUserStringInput("Enter save file name: ", 1, 60, "");
				Console.Clear();
				if (input.wasCanceled) {
					break;
				}

				if (existingSaveGames.Contains(input.result)) {
					Console.Out.WriteLine($"A save with the name {input.result} already exists!");
					if (!GetUserYnInput("Overwrite?")) {
						continue;
					}
				}

				fileName = input.result;
				break;
			}

			return fileName;
		}

		private static Key ConvertInputToDirection(ConsoleKey input)
		{
			return input switch {
				ConsoleKey.Q => Key.Exit,
				ConsoleKey.Escape => Key.Exit,
				ConsoleKey.X => Key.Exit,
				ConsoleKey.K => Key.Up,
				ConsoleKey.UpArrow => Key.Up,
				ConsoleKey.J => Key.Down,
				ConsoleKey.S => Key.Save,
				ConsoleKey.DownArrow => Key.Down,
				ConsoleKey.H => Key.Left,
				ConsoleKey.LeftArrow => Key.Left,
				ConsoleKey.L => Key.Right,
				ConsoleKey.RightArrow => Key.Right,
				_ => Key.Wrong
			};
		}
	}
}