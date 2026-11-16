using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CbrConverter
{
    /// <summary>
    /// Works out what a file really is from its first bytes, to explain why it can't be converted
    /// </summary>
    public static class FileSniffer
    {
        const int HeaderLength = 1024;          // a PDF must start within this many bytes
        const long TinyFileBytes = 10 * 1024;   // smaller than this is almost certainly a broken download

        /// <summary>
        /// True if the file starts like a PDF ("%PDF-" within the first 1 KB)
        /// </summary>
        public static bool LooksLikePdf(string path)
        {
            return Encoding.ASCII.GetString(ReadStart(path, HeaderLength)).Contains("%PDF-");
        }

        /// <summary>
        /// True if the file starts like a ZIP, RAR or 7-Zip archive
        /// </summary>
        public static bool LooksLikeArchive(string path)
        {
            string kind = Identify(ReadStart(path, HeaderLength));
            return kind != null && kind.Contains("archive");
        }

        /// <summary>
        /// A sentence saying what the file actually is, e.g. "it's an empty file (probably a failed download)"
        /// </summary>
        public static string Describe(string path)
        {
            long size = new FileInfo(path).Length;
            if (size == 0)
                return "it's an empty file (probably a failed download)";

            byte[] start = ReadStart(path, HeaderLength);
            string kind = Identify(start);
            string text = kind == null ? "its contents aren't a format the program recognises" : "it's actually " + kind;
            if (size < TinyFileBytes && (kind == null || !kind.Contains("download")))
                text += string.Format(", and only {0} — probably a failed or incomplete download", FormatSize(size));
            return text;
        }

        private static string Identify(byte[] b)
        {
            bool StartsWith(params byte[] magic) => b.Length >= magic.Length && b.Take(magic.Length).SequenceEqual(magic);

            if (StartsWith(0x25, 0x50, 0x44, 0x46))                      // %PDF
                return "a PDF";
            if (StartsWith(0x50, 0x4B, 0x03, 0x04) || StartsWith(0x50, 0x4B, 0x05, 0x06))
                return "a ZIP archive (rename it to .cbz if it holds a comic)";
            if (StartsWith(0x52, 0x61, 0x72, 0x21))                      // Rar!
                return "a RAR archive (rename it to .cbr if it holds a comic)";
            if (StartsWith(0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C))
                return "a 7-Zip archive";
            if (StartsWith(0xFF, 0xD8, 0xFF))
                return "a JPEG image";
            if (StartsWith(0x89, 0x50, 0x4E, 0x47))
                return "a PNG image";
            if (StartsWith(0x47, 0x49, 0x46, 0x38))
                return "a GIF image";
            if (StartsWith(0x41, 0x54, 0x26, 0x54, 0x46, 0x4F, 0x52, 0x4D)) // AT&TFORM
                return "a DjVu document";

            string text = Encoding.UTF8.GetString(b).TrimStart('﻿', ' ', '\t', '\r', '\n').ToLowerInvariant();
            if (text.StartsWith("<!doctype html") || text.StartsWith("<html") || (text.StartsWith("<") && text.Contains("<html")))
                return "a web page (the download probably failed)";
            if (text.Contains("%pdf-"))
                return "a PDF with other data in front of it";
            return null;
        }

        private static byte[] ReadStart(string path, int count)
        {
            using (var stream = File.OpenRead(path))
            {
                var buffer = new byte[Math.Min(count, stream.Length)];
                int read = stream.Read(buffer, 0, buffer.Length);
                return buffer.Take(read).ToArray();
            }
        }

        private static string FormatSize(long bytes)
        {
            return bytes < 1024 ? bytes + " bytes" : (bytes / 1024.0).ToString("0.#") + " KB";
        }
    }
}
