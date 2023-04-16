using System.ComponentModel.DataAnnotations;

namespace SneakerOnlineShop.Validation
{
    public class DateGreaterThanNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime inputDate = (DateTime)value;
            if (inputDate < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}
