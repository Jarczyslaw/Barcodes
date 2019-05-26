using System.Threading;

namespace Barcodes.Startup
{
    public static class SingleInstanceCheck
    {
        private const string mutexId = "7dd8f109-a7dc-4fd1-a8ec-9df1721a1c5c";
        private static Mutex mutex;

        public static bool Check()
        {
            mutex = new Mutex(true, mutexId, out bool createdNew);
            return createdNew;
        }
    }
}