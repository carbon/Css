namespace Carbon.Css.Resolver
{
	using System.IO;

	/*
	public class ICssResolver
	{
		public string Resolve();	
	}

	public class DefaultCssResolver
	{
		private readonly DirectoryInfo root;
		private readonly string scopedPath;

		public DefaultCssResolver(DirectoryInfo root, FileInfo file)
		{
			this.root = root;
			this.scopedPath = GetScopedPath(file);
		}

		public IStylesheet Resolve(CssUrlValue value)
		{
			var absolutePath = value.GetAbsolutePath(scopedPath);

			return new FileInfo(Path.Combine(root + absolutePath.Replace('/', '\\')));			
		}

		public string GetScopedPath(FileInfo file)
		{
			var scopedPath = file.Directory.FullName.Replace(root.FullName, "").Replace('\\', '/');

			if (!scopedPath.EndsWith("/"))
			{
				scopedPath += '/';
			}

			return scopedPath;
		}
	}
	*/
}
