using System;
using System.Collections.Generic;

namespace MenuSystem
{
	public class Menu
	{
		private readonly int _menuLevel;

		private const string MenuCommandExit = "X";
		private const string MenuCommandReturnToMain = "M";

		private Dictionary<string, MenuItem> _menuItemsDictionary = new Dictionary<string, MenuItem>();

		public Menu(int menuLevel = 0)
		{
			_menuLevel = menuLevel;
		}

		public string? Title { get; set; }

		public Dictionary<string, MenuItem> MenuItemsDictionary {
			get => _menuItemsDictionary;
			set {
				_menuItemsDictionary = value;
				if (_menuLevel >= 1) {
					_menuItemsDictionary.Add(MenuCommandReturnToMain,
						new MenuItem {Title = "Return to main menu"});
				}

				_menuItemsDictionary.Add(MenuCommandExit,
					new MenuItem {Title = "Exit"});
			}
		}


		public string Run()
		{
			string? command;
			do {
				PrintMenu();

				command = Console.ReadKey(false).Key.ToString().ToUpper();
				if (command.Length >= 2) {
					command = command[1].ToString();
				}

				Console.Write("\b \b"); // clear the last character

				string? returnCommand;

				if (MenuItemsDictionary.ContainsKey(command)) {
					returnCommand = RunCommand(command);
				} else {
					command = null;
					continue;
				}

				if (returnCommand == MenuCommandExit) {
					command = MenuCommandExit;
				} else if (returnCommand == MenuCommandReturnToMain && _menuLevel != 0) {
					command = MenuCommandReturnToMain;
				}
			} while (command != MenuCommandExit &&
			         command != MenuCommandReturnToMain);

			return command;
		}

		private string? RunCommand(string command)
		{
			string? result = null;
			var menuItem = MenuItemsDictionary[command];
			if (menuItem.CommandToExecute != null) {
				result = menuItem.CommandToExecute();
			}

			return result;
		}


		private void PrintMenu()
		{
			Console.Clear();
			Console.WriteLine(Title);
			Console.WriteLine("======================================");

			foreach (var menuItem in MenuItemsDictionary) {
				Console.Write(menuItem.Key);
				Console.Write(" ");
				Console.WriteLine(menuItem.Value);
			}

			Console.WriteLine("======================================");
		}
	}
}