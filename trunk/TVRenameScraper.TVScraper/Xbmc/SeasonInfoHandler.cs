using System;
using TVRenameScraper.TvScraper.LocalUtilities;
using TVRenameScraper.TvScraper.Logging;
using TVRenameScraper.TvScraper.Tvdb;

namespace TVRenameScraper.TvScraper.Xbmc
{
    public class SeasonInfoHandler
    {
        public static void CreateThumbnailArt(BannerInfo bannerInfo, string folderPath, string fileName)
        {
            ConsoleLogger.LogStart("Downloading missing season thumbnail...");
            if (!string.IsNullOrWhiteSpace(bannerInfo.Path))
            {
                DownloadManager.DownloadAndWriteFile(bannerInfo.Path, folderPath + "\\" + fileName);
            }
            else
            {
                ConsoleLogger.Warning("not found!");
            }
            ConsoleLogger.LogEnd("done.");
        }
    }
}
