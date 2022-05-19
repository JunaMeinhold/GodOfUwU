namespace GodOfUwU.Core
{
    using Octokit;
    using System;
    using System.Diagnostics;
    using System.IO.Compression;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class Updater
    {
        public static void Restart()
        {
            // Starts a new instance of the program itself
            System.Diagnostics.Process.Start(Environment.ProcessPath);

            // Closes the current process
            Environment.Exit(0);
        }

        public static async Task<int> CheckVersionAsync()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("JunaMeinhold", "GodOfUwU");

            //Setup the versions
            Version latestGitHubVersion = new(releases[0].TagName);

            Version localVersion = new(GetCurrentVersion());

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);

            return versionComparison;
        }

        public static async Task<string> GetLatestVersion()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("JunaMeinhold", "GodOfUwU");

            return releases[0].TagName;
        }

        public static string GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName()?.Version?.ToString() ?? string.Empty;
        }

        public static async Task Update()
        {
            GitHubClient client = new(new ProductHeaderValue("GodOfUwULauncher"));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("JunaMeinhold", "GodOfUwU");

            Version latestGitHubVersion = new(releases[0].TagName);

            Version localVersion = new(GetCurrentVersion());

            int versionComparison = localVersion.CompareTo(latestGitHubVersion);

            if (versionComparison >= 0)
            {
                return;
            }

            const string dir = "tmp";

            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            string platformString = OperatingSystem.IsLinux() ? "linux" : "win";
            ReleaseAsset asset = releases[0].Assets.First(x => x.Name.Contains(platformString));
            HttpClient clientweb = new();
            Stream stream = clientweb.GetStreamAsync(asset.BrowserDownloadUrl).Result;
            Stream fs = File.Create("tmp.zip");
            stream.CopyTo(fs);
            fs.Flush();
            fs.Position = 0;
            stream.Close();
            ZipArchive archive = new(fs);
            archive.ExtractToDirectory(dir);
            fs.Close();
            File.Delete("tmp.zip");

            if (OperatingSystem.IsWindows())
            {
                Process.Start("cmd.exe", "/c " + @".\tmp\update.bat");
                Environment.Exit(0);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "tmp/update.sh");
                Environment.Exit(0);
            }
        }

        public static void TestUpdateScript()
        {
            if (OperatingSystem.IsWindows())
            {
                Process.Start("cmd.exe", "/c " + @".\tmp\update.bat");
                Environment.Exit(0);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("bash", "tmp/update.sh");
                Environment.Exit(0);
            }
        }
    }
}