using System;

namespace RabbitMq_Common.Helpers
{
    public static class DateTimeHelper
    {
        public static int CalculateAge(DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            
            if (birthdate.Date > today.AddYears(-age)) 
                age--;

            return age;
        }
    }
}