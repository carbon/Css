using System.IO;

namespace Carbon.Css
{
    public interface ICssResolver
    {
        string ScopedPath { get; } // subpath ? 

        Stream Open(string absolutePath);

        // TODO: GetStreamAsync
    }
}