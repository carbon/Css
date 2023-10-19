namespace Carbon.Css.Tests;

public static class TestHelper
{
    public static FileInfo GetTestFile(string name)
    {
        string b = new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

        return new FileInfo(Path.Combine(b, "src", "Carbon.Css.Tests", "data", name));
    }
}
