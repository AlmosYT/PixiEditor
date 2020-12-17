using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Platforms;

namespace PixiEditor.ExtensionsModule
{
    public class ExtensionManager
    {
        private static List<Extension> extensions = new List<Extension>();

        public static IReadOnlyCollection<Extension> LoadedExtensions => extensions;

        public static void RegisterType<T>() => UserData.RegisterType<T>();

        private Dictionary<string, object> globals = new Dictionary<string, object>();

        public ExtensionManager(IPlatformAccessor platformAccessor)
        {
            if (platformAccessor != null)
            {
                Script.GlobalOptions.Platform = platformAccessor;
            }
        }

        public Extension LoadExtension(string path)
        {
            return LoadExtension(new DirectoryInfo(path), XDocument.Load(path + "/config.xml"));
        }

        public Extension LoadExtension(DirectoryInfo directory, XDocument document)
        {
            Extension extension = new Extension(directory, document);

            extensions.Add(extension);

            foreach (Script script in extensions.Select(x => x.ScriptFiles.Select(x => x.Value)))
            {
                foreach (KeyValuePair<string, object> pair in globals)
                {
                    script.Globals[pair.Key] = pair.Value;
                }
            }

            return extension;
        }

        /// <summary>
        /// Register an static generic which can be accessed by all scripts.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        public void RegisterGlobal<T>(string key)
        {
            if (!UserData.IsTypeRegistered<T>())
            {
                UserData.RegisterType<T>();
            }

            foreach (Script script in extensions.Select(x => x.ScriptFiles.Select(x => x.Value)))
            {
                script.Globals[key] = typeof(T);
                globals.Add(key, typeof(T));
            }
        }

        /// <summary>
        /// Registers an object which can be accessed by all scripts.
        /// </summary>
        /// <typeparam name="T">The type of generic.</typeparam>
        /// <param name="generic">The generic to be registerd</param>
        public void RegisterGlobal<T>(T generic, string key)
        {
            if (!UserData.IsTypeRegistered<T>())
            {
                UserData.RegisterType<T>();
            }

            foreach (Script script in extensions.Select(x => x.ScriptFiles.Select(x => x.Value)))
            {
                script.Globals[key] = generic;
                globals.Add(key, generic);
            }
        }
    }
}