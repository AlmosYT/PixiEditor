using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.ExtensionsModule.Exceptions
{
    public class ExtensionLoadingException : Exception
    {
        private readonly string extensionName;

        public string ExtensionName => extensionName;

        public ExtensionLoadingException(string extensionName)
        {
            this.extensionName = extensionName;
        }

        public ExtensionLoadingException(string extensionName, string message)
            : base(message)
        {
            this.extensionName = extensionName;
        }

        public ExtensionLoadingException(string extensionName, string message, Exception inner)
            : base(message, inner)
        {
            this.extensionName = extensionName;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Message} (Extension: {ExtensionName}) \n{StackTrace}" + InnerException != null ?
                $"\n ---> {InnerException}" : string.Empty;
        }
    }
}