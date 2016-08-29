namespace Carbon.Css.Tests
{
    using System;
    using System.IO;
    using System.Reflection;

    public class FixtureBase
    {
        // Assembly.GetExecutingAssembly().CodeBase

        public static readonly string ExecutingAssemblyCodeBasePath = Path.GetDirectoryName(AppContext.BaseDirectory);

        public FileInfo GetTestFile(string name)
        {
            return new FileInfo(ExecutingAssemblyCodeBasePath.Replace("file:\\", "") + "\\..\\..\\data\\" + name);
        }
    }
}
