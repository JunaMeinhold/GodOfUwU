namespace GodOfUwU.Core.Services
{
    using Discord;

    public class LogService
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly FileStream _fileStream;
        private readonly StreamWriter _writer;

        public LogService()
        {
            _semaphoreSlim = new SemaphoreSlim(1);
            if (!Directory.Exists("./logs/"))
                Directory.CreateDirectory("./logs/");
            _fileStream = File.Create("./logs/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".log");
            _writer = new(_fileStream);
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _writer.AutoFlush = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _writer.WriteLine(e.ExceptionObject.ToString());
            if (e.IsTerminating)
            {
                _writer.Flush();
                _writer.Close();
            }
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            _writer.Flush();
            _writer.Close();
        }

        public async Task LogAsync(LogMessage arg)
        {
            await _semaphoreSlim.WaitAsync();

            var timeStamp = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm tt");
            const string format = "{0,-10} {1,10}";

            string log = $"[{timeStamp}] {string.Format(format, arg.Source, $": {arg.Message}")}";
            Console.WriteLine(log);
            await _writer.WriteLineAsync(log);

            _semaphoreSlim.Release();
        }
    }
}