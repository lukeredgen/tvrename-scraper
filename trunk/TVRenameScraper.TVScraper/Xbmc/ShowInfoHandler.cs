using System.Xml.Linq;
using TVRenameScraper.TvScraper.LocalUtilities;
using TVRenameScraper.TvScraper.Logging;
using TVRenameScraper.TvScraper.Tvdb;
using TVRenameScraper.TvScraper.TVRename;

namespace TVRenameScraper.TvScraper.Xbmc
{
    public class ShowInfoHandler
    {
        public static void CreateNfoFile(SeriesInfo seriesInfo, string folderPath, string fileName, TVRenameShow tvRenameShow)
        {
            // create document
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootElem = new XElement("tvshow", new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"), new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
            

            // populate with correct nodes from series info
            if (tvRenameShow.UseCustomShowName)
            {
                rootElem.Add(new XElement("title", tvRenameShow.CustomShowName ?? string.Empty));
            }
            else
            {
                rootElem.Add(new XElement("title", seriesInfo.Title ?? string.Empty));
            }
            rootElem.Add(new XElement("id", seriesInfo.ID));
            rootElem.Add(new XElement("rating", seriesInfo.Rating ?? string.Empty));
            rootElem.Add(new XElement("mpaa", seriesInfo.MpaaClassification ?? string.Empty));
            rootElem.Add(new XElement("premiered", seriesInfo.DatePremiered ?? string.Empty));
            rootElem.Add(new XElement("studio", seriesInfo.Studio ?? string.Empty));
            rootElem.Add(new XElement("plot", seriesInfo.Plot ?? string.Empty));
            // actors
            foreach (var actorInfo in seriesInfo.Actors)
            {
                XElement actorElem = new XElement("actor");
                actorElem.Add(new XElement("name", actorInfo.Name ?? string.Empty));
                actorElem.Add(new XElement("role", actorInfo.Role ?? string.Empty));
                actorElem.Add(new XElement("thumb", actorInfo.ThumbnailUrl ?? string.Empty));
                rootElem.Add(actorElem);
            }

            infoDoc.Add(rootElem);

            // write document out to correct directory
            ConsoleLogger.LogStart("Creating NFO file...");
            if (!CustomConfiguration.DisableAllFileSystemActions)
            {
                infoDoc.Save(folderPath + "\\" + fileName);
            }

            ConsoleLogger.LogEnd("done.");
        }

        public static void CreateFanart(SeriesInfo seriesInfo, string folderPath, string fileName)
        {
            ConsoleLogger.LogStart("Downloading missing series fanart...");
            if (!string.IsNullOrWhiteSpace(seriesInfo.FanartUrl))
            {
                DownloadManager.DownloadAndWriteFile(seriesInfo.FanartUrl, folderPath + "\\" + fileName);
            }
            else
            {
                ConsoleLogger.Warning("not found!");
            }
            ConsoleLogger.LogEnd("done.");
        }

        public static void CreateFolderArt(SeriesInfo seriesInfo, string folderPath, string fileName)
        {
            ConsoleLogger.LogStart("Downloading missing series poster...");
            if (!string.IsNullOrWhiteSpace(seriesInfo.PosterUrl))
            {
                DownloadManager.DownloadAndWriteFile(seriesInfo.PosterUrl, folderPath + "\\" + fileName);
            }
            else
            {
                ConsoleLogger.Warning("not found!");
            }
            ConsoleLogger.LogEnd("done.");
        }
    }
}
