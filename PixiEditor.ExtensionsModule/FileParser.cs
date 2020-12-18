using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public FileParser(XElement element)
            : this(element.Attribute("From").Value, element.Attribute("To").Value, element.Attribute("Use").Value)
        {
        }

        public override int GetHashCode()
        {
            return FTShort.GetHashCode(StringComparison.CurrentCultureIgnoreCase);
        }
    }
}