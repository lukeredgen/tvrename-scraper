using System.Xml.Linq;
using TVRenameScraper.TvScraper.LocalUtilities;
using TVRenameScraper.TvScraper.Logging;
using TVRenameScraper.TvScraper.Tvdb;
using TVRenameScraper.TvScraper.TVRename;

namespace TVRenameScraper.TvScraper.Xbmc
{
    public class EpisodeInfoHandler
    {
        public static void CreateNfoFile(SeriesInfo seriesInfo, EpisodeInfo episodeInfo, string folderPath, string fileName, TVRenameShow tvRenameShow)
        {
            // create document
            XDocument infoDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement rootElem = new XElement("episodedetails");


            // populate with correct nodes from series info and episode info
            rootElem.Add(new XElement("title", episodeInfo.EpisodeName ?? string.Empty));
            rootElem.Add(new XElement("rating", episodeInfo.Rating > 0 ? episodeInfo.Rating.ToString() : string.Empty));
            rootElem.Add(new XElement("season", episodeInfo.SeasonNumber));
            if (tvRenameShow.UseDvdOrder)
            {
                rootElem.Add(new XElement("episode", episodeInfo.DvdEpisodeNumber));
            }
            else
            {
                rootElem.Add(new XElement("episode", episodeInfo.EpisodeNumber));
            }

            rootElem.Add(new XElement("plot", episodeInfo.Overview ?? string.Empty));
            rootElem.Add(new XElement("thumb", episodeInfo.ThumbnailUrl ?? string.Empty));
            rootElem.Add(new XElement("playcount", 0));
            rootElem.Add(new XElement("lastplayed", string.Empty));
            rootElem.Add(new XElement("credits", episodeInfo.Writer ?? string.Empty));
            rootElem.Add(new XElement("director", episodeInfo.Director ?? string.Empty));
            rootElem.Add(new XElement("aired", episodeInfo.FirstAired ?? string.Empty));
            //rootElem.Add(new XElement("premiered", episodeInfo.FirstAired ?? string.Empty));
            rootElem.Add(new XElement("mpaa", seriesInfo.MpaaClassification ?? string.Empty));
            rootElem.Add(new XElement("premiered", seriesInfo.DatePremiered ?? string.Empty));
            rootElem.Add(new XElement("studio", seriesInfo.Studio ?? string.Empty));
            
            // actors from series
            foreach (var actorInfo in seriesInfo.Actors)
            {
                XElement actorElem = new XElement("actor");
                actorElem.Add(new XElement("name", actorInfo.Name ?? string.Empty));
                actorElem.Add(new XElement("role", actorInfo.Role ?? string.Empty));
                actorElem.Add(new XElement("thumb", actorInfo.ThumbnailUrl ?? string.Empty));
                rootElem.Add(actorElem);
            }
            // actors from episode
            foreach (var actorInfo in episodeInfo.GuestActors)
            {
                XElement actorElem = new XElement("actor");
                actorElem.Add(new XElement("name", actorInfo.Name ?? string.Empty));
                actorElem.Add(new XElement("role", actorInfo.Role ?? string.Empty));
                actorElem.Add(new XElement("thumb", actorInfo.ThumbnailUrl ?? string.Empty));
                rootElem.Add(actorElem);
            }

            infoDoc.Add(rootElem);

            // write document out to correct directory
            ConsoleLogger.LogStart("Creating episode NFO file...");
            if (!CustomConfiguration.DisableAllFileSystemActions)
            {
                infoDoc.Save(folderPath + "\\" + fileName);
            }
            ConsoleLogger.LogEnd("done.");
        }
        //<episodedetails>
        //<title>My TV Episode</title>
        //<rating>10.00</rating>
        //<season>2</season>
        //<episode>1</episode>
        //<plot>he best episode in the world</plot>
        //<thumb>http://thetvdb.com/banners/episodes/164981/2528821.jpg</thumb>
        //<playcount>0</playcount>
        //<lastplayed></lastplayed>
        //<credits>Writer</credits>
        //<director>Mr. Vision</director>
        //<aired>2000-12-31</aired>
        //<premiered>2010-09-24</premiered>
        //<studio>Production studio or channel</studio>
        //<mpaa>MPAA certification</mpaa>
        //<epbookmark>200</epbookmark>  <!-- For media files containing multiple episodes,
        //                                where value is the time where the next episode begins in seconds  -->
        //<displayseason>3</displayseason>  <!-- For TV show specials, determines how the episode is sorted in the series  -->
        //<displayepisode>4096</displayepisode>
        //<actor>
        //  <name>Little Suzie</name>
        //  <role>Pole Jumper/Dancer</role>
        //</actor>

        public static void CreateThumbnail(EpisodeInfo episodeInfo, string folderPath, string fileName)
        {
            ConsoleLogger.LogStart("Downloading missing episode thumbnail...");
            if (!string.IsNullOrWhiteSpace(episodeInfo.ThumbnailUrl))
            {
                DownloadManager.DownloadAndWriteFile(episodeInfo.ThumbnailUrl, folderPath + "\\" + fileName);
            }
            else
            {
                ConsoleLogger.Warning("not found!");
            }
            ConsoleLogger.LogEnd("done.");
        }
    }
}
