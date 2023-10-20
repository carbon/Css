namespace Carbon.Css.Tests;

public class ResolverTests
{
    [Fact]
    public void A()
    {
        var text = File.ReadAllText(TestHelper.GetTestFile("webcat/all.scss").FullName);
        var expected = File.ReadAllText(TestHelper.GetTestFile("webcat/expected.txt").FullName);

        var sheet = StyleSheet.Parse(text, new CssContext());

        var data = new Dictionary<string, CssValue>
        {
            ["accentColor"] = CssValue.Parse("#20b9eb"),
            ["colorScheme"] = CssValue.Parse("light"),
            ["fontScheme"] = CssValue.Parse("serif"),
            ["textAlignment"] = CssValue.Parse("left")
        };

        sheet.SetResolver(new CssResolver("webcat"));

        var writer = new StringWriter();

        sheet.WriteTo(writer, data);

        writer.Flush();

        var output = writer.ToString();

        Assert.Equal(expected, output);
    }
}

public sealed class CssResolver(string basePath) : ICssResolver
{
    public string ScopedPath { get; } = basePath;

    public Stream Open(string absolutePath)
    {
        return TestHelper.GetTestFile("webcat/" + absolutePath).Open(FileMode.Open);
    }
}