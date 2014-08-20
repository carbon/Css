namespace Carbon.Css.Color
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct Bgra
	{
		public Bgra(uint value)
		{
			B = 0;
			G = 0;
			R = 0;
			A = 0;

			Value = value;
		}

		[FieldOffset(0)]
		public byte B;

		[FieldOffset(1)]
		public byte G;

		[FieldOffset(2)]
		public byte R;

		[FieldOffset(3)]
		public byte A;

		[FieldOffset(0)]
		public uint Value;
	}
}
