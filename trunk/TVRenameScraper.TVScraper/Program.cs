using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using TVRenameScraper.TvScraper.LocalUtilities;
using TVRenameScraper.TvScraper.Logging;
using TVRenameScraper.TvScraper.Tvdb;
using TVRenameScraper.TvScraper.TVRename;

namespace TVRenameScraper.TvScraper
{
    class Program
    {
        private const string FANART_FILE_NAME = "fanart.jpg";
        private const string FOLDER_FILE_NAME = "folder.jpg";
        private const string ALL_SEASON_FILE_NAME = "season-all.tbn";
        private const string SHOW_INFO_FILE_NAME = "tvshow.nfo";
        private const string SINGLE_SEASON_FILE_NAME_PATTERN = "season{0}.tbn";

        private static TVRenameSettings _tvRenameSettings;
        private static TvdbAccessor _theTvdb;

        private static bool _forceDvdOrder;

        private static TvdbAccessor TheTvdb
        {
            get
            {
                if (_theTvdb == null)
                {
                    _theTvdb = new TvdbAccessor();
                }
                return _theTvdb;
            }
        }


        static void Main(string[] args)
        {
            // deal with command line arguments
            if (args.Length > 0)
            {
                // use dvd order
                if (args.Contains("/dvd"))
                {
                    _forceDvdOrder = true;
                }
            }

            ConsoleLogger.Log("Launching scraper");

            ConsoleLogger.LogStart("Initialising cache...");
            XmlCache localCache = new XmlCache();
            ConsoleLogger.LogEnd("done.");

            ConsoleLogger.LogStart("Gathering settings from TV Rename...");
            _tvRenameSettings = new TVRenameSettings(CustomConfiguration.TVRenamePath);
            ConsoleLogger.LogEnd("done.");

            ConsoleLogger.LogStart("Searching for TV shows...");
            // find all the shows and their info
            IEnumerable<TVRenameShow> shows = GetAllTVShowsFromTVRename(_tvRenameSettings, _forceDvdOrder);
            ConsoleLogger.LogEnd("done.");

            // loop through each show);
            foreach (var tvRenameShow in shows)
            {
                if (Directory.Exists(tvRenameShow.FolderPath))
                {
                    SeriesInfo seriesInfo = null;
                    // attempt to get series name from cache
                    string seriesTitle = localCache.GetShowTitle(tvRenameShow.TvdbId);
                    if (string.IsNullOrWhiteSpace(seriesTitle))
                    {
                        seriesInfo = TheTvdb.GetSeriesInfo(tvRenameShow.TvdbId);
                        localCache.AddShowTitle(tvRenameShow.TvdbId, seriesInfo.Title);
                        seriesTitle = seriesInfo.Title;
                    }
                    ConsoleLogger.Highlight(seriesTitle);

                    bool needFanartJpg;
                    bool needFolderJpg;
                    bool needAllSeasonTbn;
                    bool needShowNfo;
                    DirectoryInfo showFolder = new DirectoryInfo(tvRenameShow.FolderPath);
                    IEnumerable<FileInfo> rootFileNames = showFolder.GetFiles();
                    needFanartJpg = (from p in rootFileNames where p.Name == FANART_FILE_NAME select p).Count() == 0;
                    needFolderJpg = (from p in rootFileNames where p.Name == FOLDER_FILE_NAME select p).Count() == 0;
                    needAllSeasonTbn = (from p in rootFileNames where p.Name == ALL_SEASON_FILE_NAME select p).Count() == 0;
                    needShowNfo = (from p in rootFileNames where p.Name == SHOW_INFO_FILE_NAME select p).Count() == 0;

                    // check if we need to get series info
                    if ((needShowNfo || needFolderJpg || needFanartJpg || needAllSeasonTbn) && seriesInfo == null)
                    {
                        seriesInfo = TheTvdb.GetSeriesInfo(tvRenameShow.TvdbId);
                    }
                    
                    if (needShowNfo)
                    {
                        ConsoleLogger.Log("Show Info needed for '" + seriesInfo.Title + "'.");
                        Xbmc.ShowInfoHandler.CreateNfoFile(seriesInfo, tvRenameShow.FolderPath, SHOW_INFO_FILE_NAME, tvRenameShow);
                    }
                    if (needFanartJpg)
                    {
                        ConsoleLogger.Log("Fanart needed for '" + seriesInfo.Title + "'.");
                        Xbmc.ShowInfoHandler.CreateFanart(seriesInfo, tvRenameShow.FolderPath, FANART_FILE_NAME);
                    }
                    if (needFolderJpg)
                    {
                        ConsoleLogger.Log("Folder art needed for '" + seriesInfo.Title + "'.");
                        Xbmc.ShowInfoHandler.CreateFolderArt(seriesInfo, tvRenameShow.FolderPath, FOLDER_FILE_NAME);
                    }
                    if (needAllSeasonTbn)
                    {
                        ConsoleLogger.Log("All season thumbnail needed for '" + seriesInfo.Title + "'.");
                        Xbmc.ShowInfoHandler.CreateFolderArt(seriesInfo, tvRenameShow.FolderPath, ALL_SEASON_FILE_NAME);
                    }

                    // process each season
                    if (tvRenameShow.UseFolderPerSeason)
                    {
                        var seasonDirectories = showFolder.GetDirectories(tvRenameShow.SeasonFolderName + "*").ToList();
                        // add specials folder if set in TV Rename
                        if (!string.IsNullOrEmpty(_tvRenameSettings.SpecialsFolderName))
                        {
                            seasonDirectories =
                                seasonDirectories.Concat(showFolder.GetDirectories(_tvRenameSettings.SpecialsFolderName))
                                    .ToList();
                        }
                        foreach (var seasonDirectory in seasonDirectories)
                        {
                            // get the season number and test if the thumbnail exists
                            int seasonNumber = seasonDirectory.Name.Equals(_tvRenameSettings.SpecialsFolderName) ? 0 :
                                int.Parse(seasonDirectory.Name.Replace(tvRenameShow.SeasonFolderName, string.Empty));
                            
                            // look for season thumbnail
                            string seasonThumbFileName = GetSeasonThumbnailName(seasonNumber);
                            FileInfo seasonThumb = new FileInfo(showFolder + "\\" + seasonThumbFileName);
                            if (!seasonThumb.Exists)
                            {
                                if (seriesInfo == null)
                                {
                                    seriesInfo = TheTvdb.GetSeriesInfo(tvRenameShow.TvdbId);
                                }
                                // download the missing season thumbnail
                                ConsoleLogger.LogStart("Season thumbnail needed for '" + seriesInfo.Title + "', season '" + seasonNumber + "'...");
                                BannerInfo bestMatchBanner = TheTvdb.GetSeasonBanner(seasonNumber, seriesInfo);
                                if (bestMatchBanner != null)
                                {
                                    Xbmc.SeasonInfoHandler.CreateThumbnailArt(bestMatchBanner, tvRenameShow.FolderPath, seasonThumbFileName);
                                    ConsoleLogger.LogEnd("downloaded.");
                                } else
                                {
                                    ConsoleLogger.Warning("not found!");
                                }
                            }

                            // check episodes within this season
                            FileInfo[] seasonDirectoryFiles = seasonDirectory.GetFiles();
                            foreach (var seasonDirectoryFile in seasonDirectoryFiles)
                            {
                                if (_tvRenameSettings.VideoExtensions.Contains(seasonDirectoryFile.Extension))
                                {
                                    // need to find SxxExx and extract the numbers
                                    Regex regex = new Regex("S(\\d\\d)E(\\d\\d)",RegexOptions.IgnoreCase);
                                    if (regex.IsMatch(seasonDirectoryFile.Name))
                                    {
                                        int season = int.Parse(regex.Match(seasonDirectoryFile.Name).Groups[1].Value);
                                        int episode = int.Parse(regex.Match(seasonDirectoryFile.Name).Groups[2].Value);

                                        // check if .nfo exist
                                        string episodeInfoFileName = seasonDirectoryFile.Name.Replace(seasonDirectoryFile.Extension, ".nfo");
                                        FileInfo episodeFileInfo = new FileInfo(seasonDirectory.FullName + "\\" + episodeInfoFileName);
                                        string episodeThumbFileName = seasonDirectoryFile.Name.Replace(seasonDirectoryFile.Extension, ".tbn");
                                        FileInfo episodeThumbFileInfo = new FileInfo(seasonDirectory.FullName + "\\" + episodeThumbFileName);

                                        // check if the nfo file is a valid xbmc file, and delete
                                        // it if it is not
                                        Xbmc.EpisodeInfoHandler.CheckInfoIsValid(ref episodeFileInfo, season, episode);

                                        if (!episodeThumbFileInfo.Exists || !episodeFileInfo.Exists)
                                        {
                                            if (seriesInfo == null)
                                            {
                                                seriesInfo = TheTvdb.GetSeriesInfo(tvRenameShow.TvdbId);
                                            }
                                            EpisodeInfo episodeInfo = seriesInfo.GetEpisodeInfo(season, episode,
                                                                                                    tvRenameShow.
                                                                                                        UseDvdOrder);
                                            if (episodeInfo != null)
                                            {
                                                
                                                if (!episodeFileInfo.Exists)
                                                {
                                                    ConsoleLogger.Log("Episode info missing: Season '" + season +
                                                                      "', Episode '" + episode + "'");

                                                    Xbmc.EpisodeInfoHandler.CreateNfoFile(seriesInfo, episodeInfo,
                                                                                          seasonDirectory.FullName,
                                                                                          episodeInfoFileName,
                                                                                          tvRenameShow);
                                                }
                                                if (!episodeThumbFileInfo.Exists)
                                                {
                                                    ConsoleLogger.Log("Episode thumbnail missing: Season '" + season +
                                                                      "', Episode '" + episode + "'");

                                                    Xbmc.EpisodeInfoHandler.CreateThumbnail(episodeInfo,
                                                                                            seasonDirectory.FullName,
                                                                                            episodeThumbFileName);
                                                }
                                            }
                                            else
                                            {
                                                ConsoleLogger.Warning(string.Format("Could not find any information for season '{0}', episode '{1}'.", season, episode));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // TODO how does this work? (not using folder per season)
                        ConsoleLogger.Error(string.Format("The show is not configured to use 'folder per season'. TV show (ID: '{0}' Path:'{1}')", tvRenameShow.TvdbId, tvRenameShow.FolderPath));
                    }
                }
                else
                {
                    ConsoleLogger.Error(string.Format("Could not find file path for TV show (ID: '{0}' Path:'{1}')", tvRenameShow.TvdbId, tvRenameShow.FolderPath));
                }
                // pause after each series
                Thread.Sleep(CustomConfiguration.PostSeriesDelay);
            }

            // persist the cache
            ConsoleLogger.LogStart("Saving cache...");
            localCache.SaveCache();
            ConsoleLogger.LogEnd("done.");

            // give time to look at console
            ConsoleLogger.Log("Closing...");
            Thread.Sleep(CustomConfiguration.PostCompletionDelay);
            
            // for each show
            // look in the base folder

                // assets should be in the base
                // fanart.jpg
                // folder.jpg
                // season-all.tbn
                // tvshow.nfo

            // use the folders to get a list of the seasons
            // checkd base folder for
                // season01.tbn
            // then within each folder:
                // episode with video extension
                // same but with tbn extension
                // same but with nfo extension
        }

        private static IEnumerable<TVRenameShow> GetAllTVShowsFromTVRename(TVRenameSettings tvRenameSettings, bool forceDvdOrder)
        {
            // find all the shows and their info
            List<TVRenameShow> shows = new List<TVRenameShow>();
            try
            {
                IEnumerable<XElement> tvShowElems = (from p in tvRenameSettings.SettingsXml.Descendants()
                                                     where p.Name == "MyShows"
                                                     select p).First().Elements("ShowItem");
                foreach (var tvShowElem in tvShowElems)
                {
                    shows.Add(new TVRenameShow(tvShowElem, forceDvdOrder));
                }
            }
            catch (Exception)
            {
                // LOG
                ConsoleLogger.Error("An error trying to load TV shows from TV Rename Config");
                throw;
            }
            return shows;
        }

        private static string GetSeasonThumbnailName(int seasonNumber)
        {
            return string.Format(SINGLE_SEASON_FILE_NAME_PATTERN, seasonNumber.ToString().PadLeft(2, '0'));
        }
    }
}
