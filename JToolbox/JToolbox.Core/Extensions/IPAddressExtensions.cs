using System;
using System.Net;

namespace JToolbox.Core.Extensions
{
    public static class IPAddressExtensions
    {
        public static IPAddress Add(this IPAddress address, int value)
        {
            var bytes = address.GetAddressBytes();
            TryReverse(bytes);
            var addressValue = BitConverter.ToInt32(bytes, 0);
            addressValue += value;
            var result = BitConverter.GetBytes(addressValue);
            TryReverse(result);
            return new IPAddress(result);
        }

        public static int Compare(this IPAddress address1, IPAddress address2)
        {
            var bytes1 = address1.GetAddressBytes();
            var bytes2 = address2.GetAddressBytes();

            TryReverse(bytes1);
            TryReverse(bytes2);

            var value1 = BitConverter.ToUInt32(bytes1, 0);
            var value2 = BitConverter.ToUInt32(bytes2, 0);
            if (value1 > value2)
            {
                return 1;
            }
            else if (value1 == value2)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        private static void TryReverse(byte[] array)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
        }
    }
}