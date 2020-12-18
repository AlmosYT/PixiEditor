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
        private readonly XDocument configFile;
        private readonly DirectoryInfo rootDirectory;
        private ParserCollection parsers = new ParserCollection();
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();
        private Dictionary<string, Script> scriptFiles = new Dictionary<string, Script>();
        private ExtensionInfo extensionInfo;
        
        public Dictionary<string, string> MenuItems => menuItems;

        public ParserCollection Parsers => parsers;

        public DirectoryInfo RootDirectory { get => rootDirectory; }

        public ExtensionInfo Info => extensionInfo;

        internal XDocument ConfigFile => configFile;

        internal Dictionary<string, Script> ScriptFiles => scriptFiles;

        internal Extension(DirectoryInfo rootDirectory, XDocument config)
        {
            this.rootDirectory = rootDirectory;
            configFile = config;
            List<string> modules = new List<string>();

            foreach (FileInfo info in rootDirectory.EnumerateFiles("*.lua", SearchOption.AllDirectories))
            {
                AddScriptFile(info, ref modules);
            }

            foreach (XElement menu in config.Root.Elements("Menu"))
            {
                foreach (XElement menuItem in menu.Elements("MenuItem"))
                {
                    MenuItems.Add(menu.Attribute("Name").Value + "/" + menuItem.Value, menuItem.Attribute("Click").Value);
                }
            }

            foreach (XElement parser in config.Root.Elements("Parser"))
            {
                parsers.Add(new FileParser(parser));
            }

            extensionInfo = new ExtensionInfo(config, Path.Join(rootDirectory.FullName, "config.xml"));

            ScriptLoader scriptLoader = new ScriptLoader(modules.ToArray());

            ScriptFiles.ToList().ForEach(x => x.Value.Options.ScriptLoader = scriptLoader);
        }

        public DynValue ExecuteParser(string from, string to)
        {
            string fTShort = from + ":" + to;

            if (!parsers.Contains(fTShort))
            {
                throw new ArgumentException("There was no parser for those extensions", nameof(from));
            }

            FileParser parser = parsers[fTShort];

            string[] sep = parser.Method.Split(':');

            if (sep.Length != 2)
            {
                throw new Exception();
            }

            return ExecuteFunction(sep[0], sep[1]);
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

        private void AddScriptFile(FileInfo info, ref List<string> modules)
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
    }
}