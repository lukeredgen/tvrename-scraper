using System.Xml.Linq;

namespace TVRenameScraper.TvScraper.TVRename
{
    public class TVRenameShow
    {
        public long TvdbId { get; set; }
        public string FolderPath { get; set; }
        public bool UseCustomShowName { get; set; }
        public string CustomShowName { get; set; }
        public bool UseFolderPerSeason { get; set; }
        public string SeasonFolderName { get; set; }
        public bool UseDvdOrder { get; set; }
        public bool PadSeasonToTwoDigits { get; set; }

        public TVRenameShow(XElement element)
        {
            if (element.Element("TVDBID") != null)
            {
                TvdbId = long.Parse(element.Element("TVDBID").Value);
            }
            if (element.Element("UseCustomShowName") != null)
            {
                UseCustomShowName = bool.Parse(element.Element("UseCustomShowName").Value);
            }
            if (element.Element("CustomShowName") != null)
            {
                CustomShowName = element.Element("CustomShowName").Value;
            }
            if (element.Element("FolderBase") != null)
            {
                FolderPath = element.Element("FolderBase").Value;
            }
            if (element.Element("FolderPerSeason") != null)
            {
                UseFolderPerSeason = bool.Parse(element.Element("FolderPerSeason").Value);
            }
            if (element.Element("SeasonFolderName") != null)
            {
                SeasonFolderName = element.Element("SeasonFolderName").Value;
            }
            if (element.Element("DVDOrder") != null)
            {
                UseDvdOrder = bool.Parse(element.Element("DVDOrder").Value);
            }
            if (element.Element("PadSeasonToTwoDigits") != null)
            {
                PadSeasonToTwoDigits = bool.Parse(element.Element("PadSeasonToTwoDigits").Value);
            }
        }

        public TVRenameShow(XElement element, bool forceDvdOrder) : this(element)
        {
            if (forceDvdOrder)
            {
                UseDvdOrder = true;
            }
        }

        //        <MyShows>
        //<ShowItem>
        //  <UseCustomShowName>false</UseCustomShowName>
        //  <CustomShowName></CustomShowName>
        //  <ShowNextAirdate>true</ShowNextAirdate>
        //  <TVDBID>79488</TVDBID>
        //  <AutoAddNewSeasons>true</AutoAddNewSeasons>
        //  <FolderBase>C:\TestMedia\Tv\30 Rock</FolderBase>
        //  <FolderPerSeason>true</FolderPerSeason>
        //  <SeasonFolderName>Season </SeasonFolderName>
        //  <DoRename>true</DoRename>
        //  <DoMissingCheck>true</DoMissingCheck>
        //  <CountSpecials>false</CountSpecials>
        //  <DVDOrder>false</DVDOrder>
        //  <ForceCheckNoAirdate>false</ForceCheckNoAirdate>
        //  <ForceCheckFuture>false</ForceCheckFuture>
        //  <UseSequentialMatch>false</UseSequentialMatch>
        //  <PadSeasonToTwoDigits>false</PadSeasonToTwoDigits>
        //  <IgnoreSeasons>
        //    <Ignore>0</Ignore>
        //  </IgnoreSeasons>
        //</ShowItem>
    }
}
