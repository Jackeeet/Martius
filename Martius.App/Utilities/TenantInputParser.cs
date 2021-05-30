using System;
using System.Text.RegularExpressions;
using Martius.Domain;

namespace Martius.App
{
    internal static class TenantInputParser
    {
        private static readonly Regex passportRegex = new Regex(@"^\d{4}[ -]?\d{6}$", RegexOptions.Compiled);
        private static readonly Regex phoneRegex =
            new Regex(@"^\+?\d[ -]?\(?\d{3}\)?[ -]?\d{3}[ -]?\d{2}[ -]?\d{2}$", RegexOptions.Compiled);

        internal static bool InputValid(Person person, string passport, string phone)
        {
            return person != null && passport != null && phone != null;
        }

        internal static string ParsePhone(string phoneNum)
        {
            var match = phoneRegex.Match(phoneNum);
            if (!match.Success)
                return null;

            var phone = match.Groups[0].ToString();
            return Regex.Replace(phone, @"[ \-\(\)]", string.Empty);
        }

        internal static string ParsePassport(string passNum)
        {
            var match = passportRegex.Match(passNum);
            if (!match.Success)
                return null;

            var passport = match.Groups[0].ToString();
            return Regex.Replace(passport, @"[ \-]", string.Empty);
        }

        internal static Person ParsePerson(string surname, string name, string patronym, DateTime dob)
        {
            if (DateTime.Now.AddYears(-14) < dob ||
                string.IsNullOrEmpty(surname) || surname.Length > 50 ||
                string.IsNullOrEmpty(name) || name.Length > 50 ||
                patronym.Length > 50)
            {
                return null;
            }

            return new Person(surname, name, patronym, dob);
        }
    }
}