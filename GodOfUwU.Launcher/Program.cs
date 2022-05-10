// See https://aka.ms/new-console-template for more information
using Octokit;
using System.IO.Compression;
using System.Net;

Console.WriteLine("Hello, World!");

GitHubClient client = new GitHubClient(new ProductHeaderValue("GodOfUwULauncher"));
IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("JunaMeinhold", "GodOfUwU");

//Setup the versions
Version latestGitHubVersion = new(releases[0].TagName);
Version localVersion = new("0.0.0.0"); //Replace this with your local version.
                                       //Only tested with numeric values.

string platformString = OperatingSystem.IsLinux() ? "linux" : "win";
const string dir = "bin";

int versionComparison = localVersion.CompareTo(latestGitHubVersion);
if (versionComparison < 0)
{
    if (Directory.Exists(dir))
        Directory.Delete(dir, true);
    Directory.CreateDirectory(dir);
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
}
else if (versionComparison > 0)
{
    //This local version is greater than the release version on GitHub.
}
else
{
    //This local Version and the Version on GitHub are equal.
}