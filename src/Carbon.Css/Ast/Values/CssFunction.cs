using System.IO;

namespace Carbon.Css
{
    public class CssFunction : CssValue
    {
        public CssFunction(string name, CssValue arguments)
            : base(NodeKind.Function)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; }

        public CssValue Arguments { get; }

        public override CssFunction CloneNode() => new (Name, Arguments);

        internal override void WriteTo(TextWriter writer)
        {
            writer.Write(Name);
            writer.Write('(');
            Arguments.WriteTo(writer);
            writer.Write(')');
        }

        public override string ToString()
        {
            var writer = new StringWriter();

            WriteTo(writer);

            return writer.ToString();
        }
    }
}