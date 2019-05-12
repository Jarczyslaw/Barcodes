namespace Barcodes.Utils
{
    public static class ObjectUtils
    {
        public static bool ObjectEquality<T>(T object1, T object2)
        {
            if (object1 == null && object2 == null)
            {
                return true;
            }
            else if ((object1 == null && object2 != null)
                || (object1 != null && object2 == null))
            {
                return false;
            }
            else
            {
                return object1.Equals(object2);
            }
        }
    }
}