using PixiEditor.ExtensionsModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace PixiEditorTests.ExtensionsModuleTest
{
    public class ExtensionLoaderTests
    {
        [Fact]
        public void CheckLoadingTest()
        {
            const string Name = "Test Extension";
            const string Author = "Test Author";
            const string TestDirectory = "./TestExtension";
            const string LuaTestFile = "/Test.lua";
            const string ToPrint = "Print";
            const string ToReturn = "Return";

            if (Directory.Exists(TestDirectory) && Directory.GetFiles(TestDirectory).Length != 0)
            {
                Directory.Delete(TestDirectory, true);
            }

            DirectoryInfo extensionDirectory = Directory.CreateDirectory(TestDirectory);

            using (FileStream testConfigStream = File.Create(extensionDirectory.FullName + "/config.xml"))
            {
                GetTestConfig(Name, Author).Save(testConfigStream);
            }

            using (FileStream testLuaFileStream = File.Create(extensionDirectory.FullName + LuaTestFile))
            {
                CreateTestLua(testLuaFileStream, ToPrint, ToReturn);
            }

            TestPlatformAccessor accessor = new TestPlatformAccessor();
            ExtensionManager manager = new ExtensionManager(accessor);

            Extension extension = manager.LoadExtension(extensionDirectory.FullName);

            Assert.Equal(Name, extension.Name);
            Assert.Equal(Author, extension.Author);

            string val = extension.ExecuteFunction($".{LuaTestFile}", "Test").String;

            Assert.Equal(ToPrint, accessor.Print);
            Assert.Equal(ToReturn, val);

            extensionDirectory.Delete(true);
        }

        private static XDocument GetTestConfig(string name, string author)
        {
            XDocument document = new XDocument();

            document.Add(new XElement("Config"));

            document.Root.Add(new XAttribute("Name", name), new XAttribute("Author", author));

            return document;
        }

        private static void CreateTestLua(FileStream stream, string toPrint, string toReturn)
        {
            string testLua =
                $"function Test()\n	print(\"{toPrint}\")\n	return \"{toReturn}\"\nend\n";

            using StreamWriter writer = new StreamWriter(stream);

            writer.Write(testLua);
        }
    }
}