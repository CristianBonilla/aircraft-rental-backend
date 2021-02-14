using System;
using System.IO;

namespace Rental.API
{
    public class DataDirectoryConfig
    {
        const string DataDirectory = "[DataDirectory]";
        private static readonly string directory;

        static DataDirectoryConfig()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            int lastIndex = baseDirectory.ToLower().LastIndexOf(@"bin\debug\");
            int length = lastIndex >= 0 ? lastIndex : baseDirectory.Length;
            directory = baseDirectory.Substring(0, length);
        }

        public static void SetDataDirectoryPath(ref string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "The connection string should be defined");

            string appDataPath = Path.Combine(directory, "App_Data");
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);
            connectionString = connectionString.Replace(
                DataDirectory,
                appDataPath,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
