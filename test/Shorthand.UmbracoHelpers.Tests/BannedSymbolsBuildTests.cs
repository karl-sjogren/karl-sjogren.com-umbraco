using System.Diagnostics;
using System.IO.Abstractions;
using System.IO.Compression;

namespace Shorthand.UmbracoHelpers.Tests;

public class BannedSymbolsBuildTests {
    [Fact]
    public async Task ImportedTargetsEnforceBansUnlessDisabledAsync() {
        // Arrange
        var path = new FileSystem().Path;
        var project = path.GetFullPath(path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "BuildFixtures", "Direct", "Direct.csproj"));

        // Act
        var enabled = await BuildAsync(project, disabled: false);
        var disabled = await BuildAsync(project, disabled: true);
        var reenabled = await BuildAsync(project, disabled: false);

        // Assert
        AssertBans(enabled);
        disabled.ExitCode.ShouldBe(0, disabled.Output);
        disabled.Output.ShouldNotContain("RS0030");
        AssertBans(reenabled);
    }

    [Fact]
    public async Task PackageContainsTransitiveBuildAssetsAsync() {
        // Arrange
        var fileSystem = new FileSystem();
        var path = fileSystem.Path;
        var project = path.GetFullPath(path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..", "src",
            "Shorthand.UmbracoHelpers", "Shorthand.UmbracoHelpers.csproj"));
        var outputDirectory = path.Combine(path.GetTempPath(), $"ShorthandUmbracoHelpersPackTest-{Guid.NewGuid():N}");
        fileSystem.Directory.CreateDirectory(outputDirectory);

        try {
            // Act
            var result = await RunDotnetAsync(
                "pack", project, "--configuration", "Debug", "--no-restore", "--nologo", "-v:q",
                "-p:PackageVersion=1.0.0-buildtest", "-o", outputDirectory);

            // Assert
            result.ExitCode.ShouldBe(0, result.Output);

            using var stream = fileSystem.File.OpenRead(path.Combine(outputDirectory, "Shorthand.UmbracoHelpers.1.0.0-buildtest.nupkg"));
            using var archive = new ZipArchive(stream);
            archive.GetEntry("buildTransitive/Shorthand.UmbracoHelpers.targets").ShouldNotBeNull();
            archive.GetEntry("buildTransitive/BannedSymbols.Shorthand.UmbracoHelpers.txt").ShouldNotBeNull();
        } finally {
            fileSystem.Directory.Delete(outputDirectory, recursive: true);
        }
    }

    private static void AssertBans((int ExitCode, string Output) result) {
        result.ExitCode.ShouldNotBe(0);
        result.Output.ShouldContain("RS0030");
        result.Output.ShouldContain("FriendlyImageCropperTemplateExtensions");
        result.Output.ShouldContain("FriendlyPublishedContentExtensions");
        result.Output.ShouldContain("FriendlyPublishedElementExtensions");
        result.Output.ShouldContain("FriendlyUrlHelperExtensions");
    }

    private static async Task<(int ExitCode, string Output)> BuildAsync(string project, bool disabled) {
        return await RunDotnetAsync(
            "build", project, "--nologo", "-v:q",
            $"-p:DisableShorthandUmbracoHelpersBannedSymbols={disabled.ToString().ToLowerInvariant()}");
    }

    private static async Task<(int ExitCode, string Output)> RunDotnetAsync(params string[] arguments) {
        var startInfo = new ProcessStartInfo("dotnet") {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        foreach(var argument in arguments) {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start dotnet.");
        var output = process.StandardOutput.ReadToEndAsync(TestCancellationToken);
        var error = process.StandardError.ReadToEndAsync(TestCancellationToken);

        await process.WaitForExitAsync(TestCancellationToken);
        return (process.ExitCode, (await output) + (await error));
    }
}
