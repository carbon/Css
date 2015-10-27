namespace Carbon.Css
{
    using System.IO;

    public interface IStylesheet
    {
        void WriteTo(TextWriter writer);
    }
}