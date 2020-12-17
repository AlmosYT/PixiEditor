using System;
using System.IO;
using System.Text;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Platforms;

namespace PixiEditorTests.ExtensionsModuleTest
{
    internal class TestPlatformAccessor : PlatformAccessorBase
    {
        public string Print = "";

        public override void DefaultPrint(string content)
        {
            Print = content;
        }

        public override CoreModules FilterSupportedCoreModules(CoreModules module)
        {
            return CoreModules.Basic;
        }

        public override string GetEnvironmentVariable(string envvarname)
        {
            throw new NotImplementedException();
        }

        public override string GetPlatformNamePrefix()
        {
            return "";
        }

        public override Stream IO_GetStandardStream(StandardFileType type)
        {
            throw new NotImplementedException();
        }

        public override Stream IO_OpenFile(Script script, string filename, Encoding encoding, string mode)
        {
            throw new NotImplementedException();
        }

        public override string IO_OS_GetTempFilename()
        {
            throw new NotImplementedException();
        }

        public override int OS_Execute(string cmdline)
        {
            throw new NotImplementedException();
        }

        public override void OS_ExitFast(int exitCode)
        {
            throw new NotImplementedException();
        }

        public override void OS_FileDelete(string file)
        {
            throw new NotImplementedException();
        }

        public override bool OS_FileExists(string file)
        {
            throw new NotImplementedException();
        }

        public override void OS_FileMove(string src, string dst)
        {
            throw new NotImplementedException();
        }
    }
}
