using System;

namespace Martius.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateMonthsUntil(this DateTime startDate, DateTime endDate)
        {
            return Math.Abs(12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month);
        }

        public static string GetSqlRepresentation(this DateTime date)
        {
            return "'" + date.ToString("yyyy-MM-dd") + "'";
        }
    }
}