using System.IO;

namespace Carbon.Css.Resolver;

internal readonly struct Include
{
    private readonly FileInfo _file;

    public Include(FileInfo file)
    {
        _file = file;
    }

    public DateTime Modified => _file.LastWriteTime;

    public void WriteTo(TextWriter writer)
    {
        string? line;

        using var reader = _file.OpenText();

        while ((line = reader.ReadLine()) is not null)
        {
            writer.WriteLine(line);
        }
    }
}