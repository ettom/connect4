using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core
{
	public static class Validators
	{
		public static string CreateErrorString(IEnumerable<ValidationResult> validationResults)
		{
			var sb = new StringBuilder();

			foreach (var e in validationResults) {
				sb.Append(e.ErrorMessage).Append('\n');
			}

			return sb.ToString();
		}

		public static List<ValidationResult> ValidateProperty<T>(T toValidate, string propertyName) where T : notnull
		{
			var context = new ValidationContext(toValidate) {MemberName = propertyName};
			var validationResults = new List<ValidationResult>();

			var prop = toValidate.GetType().GetProperty(propertyName)?.GetValue(toValidate);

			Validator.TryValidateProperty(prop, context, validationResults);
			return validationResults;
		}

		public class PlayerSymbolsValidator : ValidationAttribute
		{
			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				var currentValue = (List<PlayerPrototype>) value;


				var allSymbols = currentValue.Select(player => player.Symbol).Distinct().ToList();
				if (allSymbols.Count != currentValue.Count) {
					return new ValidationResult("Player symbols must be unique!");
				}

				return ValidationResult.Success;
			}
		}


		public class AIPlayersValidator : ValidationAttribute
		{
			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				var currentValue = (List<PlayerPrototype>) value;

				if (currentValue.Count(p => p.isAI) == currentValue.Count) {
					return new ValidationResult("At least one player must be human!");
				}

				return ValidationResult.Success;
			}
		}


		public class StrikeSizeValidator : ValidationAttribute
		{
			private readonly string _width;
			private readonly string _height;

			public StrikeSizeValidator(string width, string height)
			{
				_width = width;
				_height = height;
			}

			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				var currentValue = (int) value;

				var widthProperty = validationContext.ObjectType.GetProperty(_width);
				var heightProperty = validationContext.ObjectType.GetProperty(_height);

				if (heightProperty == null || widthProperty == null)
					throw new ArgumentException("Property with this name not found");

				var heightComparisonValue = (int) heightProperty.GetValue(validationContext.ObjectInstance);
				var widthComparisonValue = (int) widthProperty.GetValue(validationContext.ObjectInstance);

				const string errorMessage = "Strike size must not be greater than";

				if (currentValue > widthComparisonValue) {
					return new ValidationResult($"{errorMessage} {widthComparisonValue}.");
				}

				if (currentValue > heightComparisonValue) {
					return new ValidationResult($"{errorMessage} {heightComparisonValue}.");
				}

				if (currentValue <= 2) {
					return new ValidationResult("Strike size must be greater than 2.");
				}

				return ValidationResult.Success;
			}
		}
		
	}
}