using System;

namespace Martius.Infrastructure
{
    public static class CastUtils
    {
        public static int? ToNullableInt(string s)
        {
            return int.TryParse(s, out var n) ? n : (int?) null;
        }
        
        public static string FormatSqlDate(DateTime date)
        {
            return "'" + date.ToString("yyyy-MM-dd") + "'";
        }
        
        // to extension methods probably
        public static decimal GetDecimalPoints(decimal d, int points = 2)
        {
            return decimal.Round((decimal.Round(d, points) % 1m) * 100);
        }
    }
}