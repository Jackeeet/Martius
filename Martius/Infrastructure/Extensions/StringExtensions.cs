namespace Martius.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static int? ToNullableInt(this string s)
        {
            return int.TryParse(s, out var n) ? n : (int?) null;
        }
    }
}