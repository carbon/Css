namespace Carbon.Css.Tests
{
	using System.IO;
	using System.Reflection;

	public class FixtureBase
	{

		public static readonly string ExecutingAssemblyCodeBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

		public FileInfo GetTestFile(string name)
		{
			return new FileInfo(ExecutingAssemblyCodeBasePath.Replace("file:\\", "") + "\\..\\..\\data\\" + name);
		}

	}
}
