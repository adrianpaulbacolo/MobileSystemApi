namespace Helpers
{
    /// <summary>
    /// Summary description for BusinessRules
    /// </summary>
    public static class BusinessRules
    {

        public static void SetRules()
        {
            CheckClubApollo();
        }

        private static void CheckClubApollo()
        {
            var currency = commonVariables.GetSessionVariable("CurrencyCode");
            if (currency.Contains("AUD") || currency.Contains("VND") || currency.Contains("IDR"))
                commonVariables.SetSessionVariable("clubapollo", "0");
            else
                commonVariables.SetSessionVariable("clubapollo", "1");
        }

    }
}