#tool "nuget:?package=xunit.runner.console&version=2.4.1"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "Audio.sln";

// define directories
var buildDir = Directory("./src/Api/bin") + Directory(configuration);

Task("Clean")
    .Does(() =>
{
    foreach (var project in ParseSolution(solution).Projects) {
        DotNetCoreClean(project.Path.ToString());
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild(
        "./Audio.sln",
        new DotNetCoreBuildSettings {
            Configuration = configuration
        });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projectFiles = GetFiles("./tests/**/*.csproj");
    foreach(var file in projectFiles)
    {
        DotNetCoreTest(
            file.FullPath,
            new DotNetCoreTestSettings {
                Configuration = configuration,
                NoBuild = true
            });
    }
});

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
