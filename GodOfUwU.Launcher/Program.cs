// See https://aka.ms/new-console-template for more information
using Octokit;

Console.WriteLine("Hello, World!");

GitHubClient client = new GitHubClient(new ProductHeaderValue("GodOfUwULauncher"));
IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("JunaMeinhold", "GodOfUwU");

//Setup the versions
Version latestGitHubVersion = new(releases[0].TagName);
Version localVersion = new("X.X.X"); //Replace this with your local version.
                                     //Only tested with numeric values.

int versionComparison = localVersion.CompareTo(latestGitHubVersion);
if (versionComparison < 0)
{
    //The version on GitHub is more up to date than this local release.
}
else if (versionComparison > 0)
{
    //This local version is greater than the release version on GitHub.
}
else
{
    //This local Version and the Version on GitHub are equal.
}