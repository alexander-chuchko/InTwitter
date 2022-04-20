using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InTwitter.Validators
{
    public static class Validator
    {
        #region -- Patterns regular expressions --

        private static readonly string _patternName = @"^[a-zA-Z]+$";
        private static readonly string _patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

        #endregion

        public static bool ValidateName(string name)
        {
            return Regex.IsMatch(name, _patternName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, _patternEmail, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static bool ValidatePassword(string password)
        {
            Regex hasDigit = new Regex(@"[0-9]+");
            Regex hasUpperLetter = new Regex(@"[A-Z]+");
            Regex hasRequeriedLength = new Regex(@".{6,}");

            return hasDigit.IsMatch(password) &&
                   hasUpperLetter.IsMatch(password) &&
                   hasRequeriedLength.IsMatch(password);
        }
    }
}