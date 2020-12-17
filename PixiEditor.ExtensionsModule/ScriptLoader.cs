using System;
using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace PixiEditor.ExtensionsModule
{
    internal class ScriptLoader : ScriptLoaderBase
    {
        public ScriptLoader(string[] modules)
        {
            ModulePaths = modules;
        }

        public override object LoadFile(string file, Table globalContext)
        {
            return File.ReadAllText(file);
        }

        public override bool ScriptFileExists(string name)
        {
            return File.Exists(name);
        }
    }
}