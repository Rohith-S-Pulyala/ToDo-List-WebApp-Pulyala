using System.ComponentModel.DataAnnotations;

namespace ToDo_List_WebApp_Pulyala.Attributes
{
    public class FibonacciPointsAttribute: ValidationAttribute
    {
        private static readonly int[] AllowedPts = { 0, 1, 2, 3, 5, 8, 13, 21 }; // Fixed Set of Points for validation

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int point && Array.IndexOf(AllowedPts, point) != -1) // Input validation for one of the numbers in the array.
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "Invalid point value."); // Error message for failure.
        }
    }
}
