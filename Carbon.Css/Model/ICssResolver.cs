namespace Carbon.Css
{
    public interface ICssResolver
    {
        string ScopedPath { get; }

        string GetText(string absolutePath);

        // TODO: GetStreamAsync
    }
}