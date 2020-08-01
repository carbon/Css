using System;
using System.IO;

namespace Carbon.Css.Resolver
{
    internal readonly struct Include
    {
        private readonly FileInfo file;

        public Include(FileInfo file)
        {
            this.file = file;
        }

        public DateTime Modified => file.LastWriteTime;

        public void WriteTo(TextWriter writer)
        {
            string? line;

            using var reader = file.OpenText();

            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);
            }
        }
    }
}