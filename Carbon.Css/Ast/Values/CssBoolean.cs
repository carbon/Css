namespace Carbon.Css
{
	public class CssBoolean : CssValue
	{
		private readonly bool value;

		public CssBoolean(bool value)
			: base(NodeKind.PrimitiveValue)
		{
			this.value = value;
		}
		
		public bool Value => value;

		public override CssNode CloneNode() => new CssBoolean(value);

		public override string ToString() => value.ToString().ToLower();
	}
}