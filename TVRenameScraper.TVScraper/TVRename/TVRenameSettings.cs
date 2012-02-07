using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TVRenameScraper.TvScraper.Logging;

namespace TVRenameScraper.TvScraper.TVRename
{
    public class TVRenameSettings
    {
        public XDocument SettingsXml;
        public string NamingStyle { get; set; }
        public List<string> VideoExtensions { get; set; }
        public List<string > OtherExtensions { get; set; }

        public TVRenameSettings(string tvRenameSettingsXmlpath)
        {
            try
            {
                SettingsXml = XDocument.Load(tvRenameSettingsXmlpath);
            }
            catch (Exception)
            {
                // LOG 
                ConsoleLogger.Error(String.Format("Could not find or load TV Rename settings file (path : '{0}')",
                                                tvRenameSettingsXmlpath));
            }

            //<NamingStyle>{ShowName} - S{Season:2}E{Episode}[-E{Episode2}] - {EpisodeName}</NamingStyle>
            try
            {
                XElement namingStyleElem = (from p in SettingsXml.Descendants()
                                            where p.Name == "NamingStyle"
                                            select p).First();
                NamingStyle = namingStyleElem.Value;
            }
            catch (Exception)
            {
                // LOG
                ConsoleLogger.Error("Could not find naming style element in TV Rename Config");
                throw;
            }
            //<VideoExtensions>.avi;.mpg;.mpeg;.mkv;.mp4;.wmv;.divx;.ogm;.qt;.rm</VideoExtensions>
            try
            {
                XElement videoExtensionsElem = (from p in SettingsXml.Descendants()
                                                where p.Name == "VideoExtensions"
                                                select p).First();
                VideoExtensions = videoExtensionsElem.Value.Split(';').ToList();
            }
            catch (Exception)
            {
                // LOG
                ConsoleLogger.Error("Could not find video extensions element in TV Rename Config");
                throw;
            }
            //<OtherExtensions>.srt;.nfo;.txt;.tbn</OtherExtensions>
            try
            {
                XElement otherExtensionsElem = (from p in SettingsXml.Descendants()
                                                where p.Name == "OtherExtensions"
                                                select p).First();
                OtherExtensions = otherExtensionsElem.Value.Split(';').ToList();
            }
            catch (Exception)
            {
                // LOG
                ConsoleLogger.Error("Could not find other extensions element in TV Rename Config");
                throw;
            }
        }
    }
}
