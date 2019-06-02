using NamedPipeWrapper;
using System;
using System.Threading;

namespace Barcodes.SingleInstance
{
    public class SingleInstanceManager : IDisposable
    {
        private static Mutex mutex;

        private readonly string appKey;

        private NamedPipeServer<object> server;

        public SingleInstanceManager(string appKey, TimeSpan timeout)
        {
            this.appKey = appKey;
            Check(timeout);
        }

        public delegate void NewInstanceHandler();

        public event NewInstanceHandler OnNewInstance;

        public bool FirstInstance { get; private set; }

        public void Dispose()
        {
            Release();
        }

        public void Release()
        {
            if (FirstInstance)
            {
                mutex.ReleaseMutex();
                server.ClientConnected -= Server_ClientConnected;
                server.Stop();
            }
        }

        public void SendNofitication()
        {
            StartClient();
        }

        private bool Check(TimeSpan timeout)
        {
            mutex = new Mutex(true, appKey);
            FirstInstance = mutex.WaitOne(timeout);
            if (FirstInstance)
            {
                StartServer();
            }
            return FirstInstance;
        }

        private void Server_ClientConnected(NamedPipeConnection<object, object> connection)
        {
            OnNewInstance?.Invoke();
        }

        private void StartClient()
        {
            var client = new NamedPipeClient<object>(appKey);
            client.Start();
            client.WaitForConnection();
            client.Stop();
        }

        private void StartServer()
        {
            server = new NamedPipeServer<object>(appKey);
            server.ClientConnected += Server_ClientConnected;
            server.Start();
        }
    }
}