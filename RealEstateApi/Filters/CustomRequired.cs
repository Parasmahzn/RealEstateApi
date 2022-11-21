using System.ComponentModel.DataAnnotations;

namespace RealEstateApi.Filters
{
    public class CustomRequired : RequiredAttribute
    {
        public string ErrorCode;
        public CustomRequired()
        {
            ErrorCode = "";
        }

        public override bool IsValid(object? value)
        {
            return base.IsValid(value);
        }
    }
}
