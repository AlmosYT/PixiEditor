using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PixiEditor.ExtensionsModule
{
    public class ExtensionInfo
    {
        public string Name { get; private set; }

        public string Author { get; private set; } = "Unknown Author";

        public string Description { get; private set; } = string.Empty;

        public static ExtensionInfo Load(string path)
        {
            if (!Helpers.PathExists(path))
            {
                throw new FileNotFoundException("No config file found at the specified path", Path.GetFullPath(path));
            }

            if (Helpers.IsDirectory(path))
            {
                path = Path.Join(path, "config.xml");
            }

            return new ExtensionInfo(XDocument.Load(path), path);
        }

        public ExtensionInfo(XDocument document, string path)
        {
            string name = document.Root.Attribute("Name")?.Value;
            string author = document.Root.Attribute("Author")?.Value;
            string description = document.Root.Element("Description")?.Value;

            if (name != null)
            {
                Name = name;
            }
            else
            {
                FileInfo file = new FileInfo(path);

                Name = file.Directory.Name;
            }

            if (author != null)
            {
                Author = author;
            }

            if (description != null)
            {
                Description = description;
            }
        }
    }
}