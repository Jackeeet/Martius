using System;

namespace Martius.Infrastructure
{
    public static class CastUtils
    {
        public static int? ToNullableInt(string s) => int.TryParse(s, out var n) ? n : (int?) null;

        public static string FormatSqlDate(DateTime date) => "'" + date.ToString("yyyy-MM-dd") + "'";
    }
}