namespace Carbon.Color
{
	using System;

	internal static class HexString
	{
		internal static byte[] ToBytes(string hexString)
		{
			var bytes = new byte[hexString.Length / 2];

			for (int i = 0; i < hexString.Length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
			}

			return bytes;
		}
	}
}
