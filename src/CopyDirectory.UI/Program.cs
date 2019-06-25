using System;
using System.Threading.Tasks;
using CopyDirectory.Lib;

namespace CopyDirectory.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fileCopier = new FileCopier(new ConsoleLogger());

            var sourcePath = "TestData/Source";
            var targetPath = "TestData/Target";

            try
            {
                await fileCopier.CopyDirectoryAsync(sourcePath, targetPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
