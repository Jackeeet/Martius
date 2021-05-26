using System.Text.RegularExpressions;
using Martius.Domain;
using Martius.Infrastructure;

namespace Martius.App
{
    internal static class InputValidator
    {
        private static readonly Regex bldNumRegex = new Regex(@"^(\d{1,5})(\D*)$", RegexOptions.Compiled);
        private static readonly Regex aptNumRegex = new Regex(@"^(\d{1,5})$", RegexOptions.Compiled);

        internal static bool InputValid(Address address, bool roomsParsed, bool areaParsed, bool priceParsed)
        {
            return address != null && roomsParsed && areaParsed && priceParsed;
        }

        internal static bool AmountsValid(int roomCount, double area, decimal price)
        {
            return roomCount > 0 && area > 0.0d && price > decimal.Zero;
        }

        internal static Address ParseAddress(string city, string street, string bld, string apt)
        {
            var buildingMatch = bldNumRegex.Match(bld);
            var apartmentMatch = aptNumRegex.Match(apt);
            if (!AddressCorrect(buildingMatch, apartmentMatch, city, street, apt))
                return null;

            var building = ParseBuildingNumber(buildingMatch);
            var aptNum = ParseApartmentNumber(apartmentMatch);
            return building.Number == 0 || aptNum == 0
                ? null
                : new Address(city, street, building.Number, aptNum, building.Extra);
        }

        private static bool AddressCorrect(Match bld, Match apt, string city, string street, string aptNum)
        {
            var aptParsed = apt.Success || string.IsNullOrEmpty(aptNum);

            return bld.Success && aptParsed &&
                   !string.IsNullOrEmpty(city) && city.Length <= 50 &&
                   !string.IsNullOrEmpty(street) && street.Length <= 50;
        }

        private static int? ParseApartmentNumber(Match match)
        {
            var numGroup = match.Groups[1].ToString();
            return CastUtils.ToNullableInt(numGroup);
        }

        private static BuildingNumber ParseBuildingNumber(Match match)
        {
            var numGroup = match.Groups[1].ToString();
            var buildNum = int.Parse(numGroup);
            var extra = match.Groups[2].ToString();
            return new BuildingNumber(buildNum, extra);
        }

        private class BuildingNumber
        {
            public readonly int Number;
            public readonly string Extra;

            public BuildingNumber(int num, string extra)
            {
                Number = num;
                Extra = extra;
            }
        }
    }
}