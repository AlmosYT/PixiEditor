using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.ExtensionsModule
{
    internal static class Helpers
    {
        public static bool IsDirectory(string path)
        {
            FileAttributes attributes = File.GetAttributes(path);

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                return true;
            }

            return false;
        }

        public static bool PathExists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }
    }
}