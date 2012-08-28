using System;
using System.IO;
using System.Xml.Serialization;

namespace TVRenameScraper.TvScraper.LocalUtilities
{
    public class XmlCache
    {
        private const string XML_PATH = "LocalCache.xml";
        private const string FOLDER_NAME = "TVRenameScraper";
        private SerializableDictionary<long, string> _tvdbShowTitles = new SerializableDictionary<long, string>();

        private static string XmlFullFilePath
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + FOLDER_NAME + "\\" + XML_PATH; }
        }

        public XmlCache()
        {
            try
            {
                // ensure directory exists in userdata
                DirectoryInfo folder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + FOLDER_NAME);
                if (!folder.Exists)
                {
                    folder.Create();
                }
                FileInfo cacheFile = new FileInfo(XmlFullFilePath);
                if (cacheFile.Exists)
                {
                    try
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(SerializableDictionary<long, string>));
                        using (FileStream fs = File.Open(
                            XmlFullFilePath,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.Read))
                        {
                            _tvdbShowTitles = (SerializableDictionary<long, string>) ser.Deserialize(fs);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Could Not Serialize object to " + XmlFullFilePath, ex);
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveCache()
        {
            // persist to XML
            //serialize and persist it to it's file 
            try
            {
                XmlSerializer ser = new XmlSerializer(_tvdbShowTitles.GetType());
                using (FileStream fs = File.Open(
                        XmlFullFilePath,
                        FileMode.OpenOrCreate,
                        FileAccess.Write,
                        FileShare.ReadWrite))
                {
                    ser.Serialize(fs, _tvdbShowTitles);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could Not Serialize object to " + XmlFullFilePath, ex);
            } 
        }

        public static void RemoveCache()
        {
            try
            {
                FileInfo cacheFile = new FileInfo(XmlFullFilePath);
                if (cacheFile.Exists)
                {
                    cacheFile.Delete();
                }
            }
            catch { }
        }

        public void AddShowTitle(long tvdbId, string title)
        {
            _tvdbShowTitles.Add(tvdbId, title);
        }

        public string GetShowTitle(long tvdbId)
        {
            return _tvdbShowTitles.ContainsKey(tvdbId) ? _tvdbShowTitles[tvdbId] : null;
        }
    }
}
