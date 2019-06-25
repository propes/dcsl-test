using System;
using System.Threading.Tasks;
using CopyDirectory.Lib;

namespace CopyDirectory.UI
{
    class Program
    {
        static void Main()
        {
            var fileCopier = new FileCopier(new ConsoleLogger());

            var sourcePath = "TestData/Source";
            var targetPath = "TestData/Target";

            try
            {
                // Normally I would use a third-party async library such as Nito.AsyncEx to properly unwrap any
                // exceptions thrown by the async method but haven't done this due to time constraints and not
                // knowing if there is a library compatible with dotnet core. 
                Task.Run(() => fileCopier.CopyDirectoryAsync(sourcePath, targetPath)).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
