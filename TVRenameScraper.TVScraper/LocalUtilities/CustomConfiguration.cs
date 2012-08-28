using System.Configuration;

namespace TVRenameScraper.TvScraper.LocalUtilities
{
    public static class CustomConfiguration
    {
        public static int PostCompletionDelay
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["PostCompletionDelay"]);
                }
                catch
                {
                    return 1000;
                }
            }
        }

        public static int PostSeriesDelay
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["PostSeriesDelay"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static bool DisableAllFileSystemActions
        {
            get
            {
                try
                {
                    return bool.Parse(ConfigurationManager.AppSettings["DisableAllActions"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool GetFolderJpgForSeasons
        {
            get
            {
                try
                {
                    return bool.Parse(ConfigurationManager.AppSettings["GetFolderJpgForSeasons"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string TVRenamePath
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["TVRenamePath"];
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }
}