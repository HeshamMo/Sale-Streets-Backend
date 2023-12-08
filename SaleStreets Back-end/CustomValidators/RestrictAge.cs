using System.ComponentModel.DataAnnotations;

namespace SaleStreetProject.CustomValidators
{
    public class RestrictAge:ValidationAttribute
    {
        private readonly int minAge;
        public RestrictAge(int minAge)
        {
            this.minAge = minAge;
        }
        public override bool IsValid(object? value)
        {
            try
            {

            if ((int)value >=minAge)
            {
                return true; 
            }
            else
            {
                return false;
            }
            }
            catch
            {
                return false; 
            }
        }
    }
}
