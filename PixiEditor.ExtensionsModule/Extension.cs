using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MoonSharp.Interpreter;

namespace PixiEditor.ExtensionsModule
{
    public class Extension
    {
        internal readonly XDocument ConfigFile;
        private readonly DirectoryInfo rootDirectory;
        internal Dictionary<string, Script> ScriptFiles = new Dictionary<string, Script>();
        public Dictionary<string, string> MenuItems = new Dictionary<string, string>();
        public readonly string Name;
        public readonly string Author;

        public DirectoryInfo RootDirectory { get => rootDirectory; }

        internal Extension(DirectoryInfo rootDirectory, XDocument config)
        {
            this.rootDirectory = rootDirectory;
            ConfigFile = config;
            List<string> modules = new List<string>();

            foreach (FileInfo info in rootDirectory.EnumerateFiles("*.lua", SearchOption.AllDirectories))
            {
                modules.Add(info.Name);

                string relativePath = "/" + info.Name;

                DirectoryInfo currentDir = info.Directory;

                while (true)
                {
                    if (currentDir.FullName == rootDirectory.FullName)
                    {
                        relativePath = relativePath.Insert(0, ".");
                        break;
                    }

                    relativePath += "/" + currentDir.Name;

                    currentDir = currentDir.Parent;
                }

                Script script = new Script();

                var val = script.DoFile(info.FullName);

                ScriptFiles.Add(relativePath, script);
            }

            foreach (XElement menu in config.Elements("Menu"))
            {
                foreach (XElement menuItem in menu.Elements("MenuItem"))
                {
                    MenuItems.Add(menu.Attribute("Name").Value + "/" + menuItem.Value, menu.Attribute("Click").Value);
                }
            }

            Name = config.Root.Attribute("Name")?.Value;

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = rootDirectory.Name;
            }

            Author = config.Root.Attribute("Author")?.Value;

            if (Author == null)
            {
                Author = "Unknown Author";
            }

            ScriptFiles.ToList().ForEach(x => x.Value.Options.ScriptLoader = new ScriptLoader(modules.ToArray()));
        }

        public DynValue ExecuteMenuItem(string menuItem)
        {
            if (!MenuItems.ContainsKey(menuItem))
            {
                throw new ArgumentNullException(nameof(menuItem), "There was no menu item with that name");
            }

            string[] sep = MenuItems[menuItem].Split(':');

            if (sep.Length != 2)
            {
                throw new Exception();
            }

            return ExecuteFunction(sep[0], sep[1]);
        }

        public DynValue ExecuteFunction(string file, string method, params object[] parm)
        {
            return ScriptFiles[file].Call(ScriptFiles[file].Globals[method], parm);
        }
    }
}