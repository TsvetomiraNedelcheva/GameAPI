using System.Globalization;

namespace Data
{
    public class CurrencyCode
    {
        static List<string> currencyCodes;
        static CurrencyCode()
        {
            currencyCodes = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol)
                .Distinct()
                .OrderBy(x => x).ToList();
        }

        public static bool IsValidCode(string currencyCode)
        {
            return currencyCodes.Where(x => x.Contains(currencyCode)).Any();
        }
    }
}
