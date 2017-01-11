using System.IO;

namespace Carbon.Css
{
    public interface ICssResolver
    {
        string ScopedPath { get; }

        Stream Open(string absolutePath);

        // TODO: GetStreamAsync
    }
}