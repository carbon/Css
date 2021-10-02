using System.IO;

namespace Carbon.Css;

public interface IStylesheet
{
    void WriteTo(TextWriter writer);
}