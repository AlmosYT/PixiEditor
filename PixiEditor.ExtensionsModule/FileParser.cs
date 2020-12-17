using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.ExtensionsModule
{
    public class FileParser
    {
        private readonly string fromExtension;
        private readonly string toExtension;
        private readonly string method;

        public string FTShort => fromExtension + ":" + toExtension;

        public string FromExtension => fromExtension;

        public string ToExtension => toExtension;

        public string Method => method;

        public FileParser(string from, string to, string method)
        {
            fromExtension = from;
            toExtension = to;
            this.method = method;
        }

        public override int GetHashCode()
        {
            return FTShort.GetHashCode(StringComparison.CurrentCultureIgnoreCase);
        }
    }
}