namespace Martius.Infrastructure
{
    public static class CastUtils
    {
        public static int? ToNullableInt(string s)
        {
            return int.TryParse(s, out var n) ? n : (int?) null;
        }
    }
}