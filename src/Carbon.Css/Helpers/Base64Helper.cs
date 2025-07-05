using System.IO;

namespace Carbon.Css.Helpers;

internal static class Base64Helper
{
    public static void WriteStreamAsBase64(Stream stream, TextWriter writer)
    {
        Span<byte> inputBuffer = stackalloc byte[3 * 256];
        Span<char> outputBuffer = stackalloc char[4 * 256];

        while (true)
        {
            int bytesRead = stream.ReadAtLeast(inputBuffer, inputBuffer.Length, throwOnEndOfStream: false);

            if (bytesRead is 0) break; // nothing left to encode

            Convert.TryToBase64Chars(inputBuffer[..bytesRead], outputBuffer, out int charsWritten);
            writer.Write(outputBuffer[..charsWritten]);

            if (bytesRead < inputBuffer.Length) break; // final block
        }
    }
}