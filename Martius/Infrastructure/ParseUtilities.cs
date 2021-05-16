namespace Martius.Infrastructure
{
    public class ParseUtilities
    {
        public static int? ToNullableInt(string s) => int.TryParse(s, out var n) ? n : (int?) null;
    }
}