namespace Barcodes.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsOnlyDigits(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
