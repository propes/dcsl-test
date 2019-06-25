using System;
using System.IO;
using System.Threading.Tasks;

namespace CopyDirectory.Lib
{
    public class FileCopier : IFileCopier
    {
        // A future improvement would be to override this field using a constructor argument
        // or method parameter.
        private readonly bool _overwrite = true;
        private readonly ILogger _logger;

        public FileCopier(ILogger logger)
        {
            _logger = logger;
        }

        public async Task CopyDirectoryAsync(string sourcePath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                throw new ArgumentNullException(nameof(sourcePath), "Source path cannot be null or blank.");

            if (!Directory.Exists(sourcePath))
                throw new InvalidOperationException($"Source path '{sourcePath}' does not exist");

            if (string.IsNullOrWhiteSpace(targetPath))
                throw new ArgumentNullException(nameof(targetPath), "Target path cannot be null or blank.");

            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            foreach (string filePath in Directory.EnumerateFiles(sourcePath))
            {
                using (var sourceStream = File.Open(filePath, FileMode.Open))
                {
                    var targetFile = filePath.Replace(sourcePath, targetPath);
                    using (var destinationStream = File.Create(targetFile))
                    {
                        _logger.Log($"Copying '{filePath}' to '{targetFile}'.");
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }
            }

            foreach (string subfolderPath in Directory.EnumerateDirectories(sourcePath))
            {
                var targetSubfolder = subfolderPath.Replace(sourcePath, targetPath);

                Directory.CreateDirectory(targetSubfolder);
                await CopyDirectoryAsync(subfolderPath, targetSubfolder);
            }
        }
    }
}