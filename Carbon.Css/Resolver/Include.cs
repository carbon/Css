namespace Carbon.Css.Resolver
{
	using System;
	using System.IO;

	public class Include
	{
		private readonly FileInfo file;

		public Include(FileInfo file)
		{
			this.file = file;
		}

		public DateTime Modified
		{
			get { return file.LastWriteTime; }
		}

		public void WriteTo(TextWriter writer)
		{
			using (var reader = new StreamReader(file.FullName, true))
			{
				writer.Write(reader.ReadToEnd());				
			}
		}
	}
}
