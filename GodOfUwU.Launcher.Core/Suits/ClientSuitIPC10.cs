namespace GodOfUwU.Launcher.Core.Suits
{
    using GodOfUwU.Launcher.Core.Protocol.Records;
    using System.Net;
    using System.Net.Sockets;

    public class ClientSuitIPC10
    {
        private readonly bool _connected;
        private readonly Task receiveTask;
        private readonly Socket socket;

        public Func<ApplicationData, Task>? OnAppData;

        public ClientSuitIPC10(Socket socket)
        {
            this.socket = socket;
            receiveTask = new Task(Receive, TaskCreationOptions.LongRunning);
        }

        public async Task ConnectAsync(IPEndPoint point)
        {
            await socket.ConnectAsync(point);
            receiveTask.Start();
        }

        private void Receive()
        {
        }
    }
}