namespace Martius.Infrastructure.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal GetDecimalPoints(this decimal d)
        {
            return decimal.Round((decimal.Round(d, 2) % 1m) * 100);
        }
    }
}