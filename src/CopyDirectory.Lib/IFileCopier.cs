using System.Threading.Tasks;

namespace CopyDirectory.Lib
{
    public interface IFileCopier
    {
        Task CopyDirectoryAsync(string sourcePath, string targetPath);
    }
}