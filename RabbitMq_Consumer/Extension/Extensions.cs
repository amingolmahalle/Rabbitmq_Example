using System;
using System.Text.RegularExpressions;

namespace RabbitMq_Consumer.Extension
{
    public static class Extensions
    {
        public static bool IsValidMobileNumber(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            return Regex.IsMatch(input, @"(^09\d{9}$)|(^\+989\d{9}$)|(^9\d{9}$)|(^989\d{9}$)");
        }

        public static bool IsValidEmail(this string strIn)
        {
            if (string.IsNullOrWhiteSpace(strIn))
                return false;
            try
            {
                return Regex.IsMatch(
                    strIn,
                    @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }
    }
}