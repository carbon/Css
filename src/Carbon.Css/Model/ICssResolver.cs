using System.IO;

namespace Carbon.Css
{
    public interface ICssResolver
    {
        string ScopedPath { get; } // BasePath???

        Stream Open(string absolutePath);

        // TODO: GetStreamAsync
    }
}