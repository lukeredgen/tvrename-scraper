using System;
using System.IO;
using System.Text;
using TVRenameScraper.TvScraper.Logging;

namespace TVRenameScraper.TvScraper.LocalUtilities
{
    public static class OtherDataCache
    {
        private static string _versionCacheFilePath = "VersionLastChecked.txt";
        private const string FOLDER_NAME = "TVRenameScraper";

        private static string VersionCacheFullFilePath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + FOLDER_NAME + "\\" +
                       _versionCacheFilePath;
            }
        }

        public static DateTime LastCheckedForNewVersionDateTime
        {
            get {
                try
                {
                    FileInfo cacheFile = new FileInfo(VersionCacheFullFilePath);
                    if (cacheFile.Exists)
                    {
                        using (var stream = cacheFile.OpenRead())
                        {
                            StreamReader reader = new StreamReader(stream);
                            return DateTime.Parse(reader.ReadToEnd());
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }
                return DateTime.MinValue;
            }
            set
            {
                try
                {
                    // ensure directory exists in userdata
                    DirectoryInfo folder =
                        new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" +
                                          FOLDER_NAME);
                    if (!folder.Exists)
                    {
                        folder.Create();
                    }
                    FileInfo cacheFile = new FileInfo(VersionCacheFullFilePath);
                    if (cacheFile.Exists)
                    {
                        cacheFile.Delete();
                    }
                    // create new cache file with value in it
                    using (StreamWriter x = cacheFile.CreateText())
                    {
                        x.WriteLine(value.ToString());
                        x.Close();
                    }
                }
                catch (Exception exception)
                {
                    ConsoleLogger.Error(exception.Message);
                }
            }
        }
    }
}
