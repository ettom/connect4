using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using static LoadSave.SaveGameHandler;

namespace WebApp.Pages.Game
{
	public class SaveName
	{
		[MinLength(1)]
		[MaxLength(32)]
		[Display(Prompt = "Enter a name for the save")]
		[SaveGameNameValidator("OverWrite")]
		public string Name { get; set; } = default!;

		[BindProperty] public bool OverWrite { get; set; }
	}

	public class SaveGameNameValidator : ValidationAttribute
	{
		private readonly string _overWrite;

		public SaveGameNameValidator(string overWrite)
		{
			_overWrite = overWrite;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var currentValue = (string) value;

			var overWriteProperty = validationContext.ObjectType.GetProperty(_overWrite);

			if (overWriteProperty == null) {
				throw new ArgumentException("Property with this name not found");
			}

			var overWriteValueObject = overWriteProperty.GetValue(validationContext.ObjectInstance);
			if (overWriteValueObject == null) {
				throw new ArgumentException("Property value must not be null");
			}

			var overWriteValue = (bool) overWriteValueObject;

			// don't check existing saves if allowed to overwrite
			if (overWriteValue) {
				return ValidationResult.Success;
			}

			const string errorMessage = "A saved game with this name already exists!";

			if (GetSaveGameNames().Contains(currentValue)) {
				return new ValidationResult(errorMessage);
			}

			return ValidationResult.Success;
		}
	}
}