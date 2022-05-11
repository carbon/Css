namespace Carbon.Css.Tests;

public class ResolverTests
{
    [Fact]
    public void A()
    {
        var text = TestHelper.GetTestFile("webcat/all.scss").OpenText().ReadToEnd();
        var expected = TestHelper.GetTestFile("webcat/expected.txt").OpenText().ReadToEnd();

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

        Assert.Equal(expected, writer.ToString());
    }
}

public sealed class CssResolver : ICssResolver
{
    private readonly string basePath;

    public CssResolver(string basePath)
    {
        this.basePath = basePath;
    }

    public string ScopedPath => basePath;

    public Stream Open(string absolutePath)
    {
        return TestHelper.GetTestFile("webcat/" + absolutePath).Open(FileMode.Open);
    }
}