namespace Carbon.Css
{
	public sealed class CssNumber : CssValue
	{
		private readonly float value;

		public CssNumber(float value)
			: base(NodeKind.Number) 
		{ 
			this.value = value;
		}
		
		public float Value => value;

		public override CssNode CloneNode() => new CssNumber(value);

		public override string ToString() => value.ToString();
	}
}