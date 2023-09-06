using System.Text.Json.Serialization;

namespace Carbon.Css;

public abstract class CssNode(NodeKind kind, CssNode? parent = null)
{
    [JsonIgnore]
    public NodeKind Kind { get; } = kind;

    [JsonIgnore]
    public CssNode? Parent { get; set; } = parent;

    [JsonIgnore]
    internal Trivia? Leading { get; init; }

    [JsonIgnore]
    public Trivia? Trailing { get; set; }

    public virtual CssNode CloneNode()
    {
        throw new NotImplementedException($"{GetType().Name} does not implement Clone");
    }
}