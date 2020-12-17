using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.ExtensionsModule
{
    public class ParserCollection : IEnumerable<FileParser>
    {
        private Dictionary<string, FileParser> parsers = new Dictionary<string, FileParser>();

        public FileParser this[string key] { get => parsers[key]; set => parsers[key] = value; }

        public int Count => parsers.Count;

        public void Add(FileParser parser)
        {
            parsers.Add(parser.FTShort, parser);
        }

        public void Clear()
        {
            parsers.Clear();
        }

        public bool Contains(string ftshort)
        {
            return parsers.ContainsKey(ftshort);
        }

        public bool Contains(FileParser parser)
        {
            return parsers.ContainsKey(parser.FTShort);
        }

        public IEnumerator<FileParser> GetEnumerator()
        {
            return parsers.Values.GetEnumerator();
        }

        public bool Remove(string ftshort)
        {
            return parsers.Remove(ftshort);
        }

        public bool Remove(FileParser parser)
        {
            return parsers.Remove(parser.FTShort);
        }

        public bool TryGetValue(string ftshort, [MaybeNullWhen(false)] out FileParser parser)
        {
            return parsers.TryGetValue(ftshort, out parser);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parsers.Values.GetEnumerator();
        }
    }
}
