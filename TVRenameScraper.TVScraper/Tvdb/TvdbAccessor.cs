using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using TVRenameScraper.TvScraper.Compression;
using TVRenameScraper.TvScraper.Logging;

namespace TVRenameScraper.TvScraper.Tvdb
{
    public class TvdbAccessor
    {
        private const string API_KEY = "2AE42B1208EF0AD5";
        private const string ENGLISH_CODE = "en";
        private const string GET_MIRRORS_URL_FORMAT = "http://www.thetvdb.com/api/{0}/mirrors.xml";
        private const string GET_SERIES_INFO_URL_FORMAT = "{0}/api/{1}/series/{2}/all/{3}.zip";

        #region properties

        private string MirrorsUrl
        {
            get { return string.Format(GET_MIRRORS_URL_FORMAT, API_KEY); }
        }

        private string GetSeriesInfoUrl(long tvdbId)
        {
            return string.Format(GET_SERIES_INFO_URL_FORMAT, selectedZipMirror, API_KEY, tvdbId, ENGLISH_CODE);
        }

        private string GetBannerInfoUrl(int tvdbId)
        {
            return string.Format(GET_SERIES_INFO_URL_FORMAT, selectedZipMirror, API_KEY, tvdbId, ENGLISH_CODE);
        }

        private IList<string> XmlMirrors = new List<string>();
        private IList<string> BannerMirrors = new List<string>();
        private IList<string> ZipMirrors = new List<string>();

        private string selectedXmlMirror;
        private string selectedBannerMirror;
        private string selectedZipMirror;

        #endregion

        public TvdbAccessor()
        {
            ConsoleLogger.LogStart("Initialising The TVDB...");
            // get mirrors
            PopulateMirrors();
            ConsoleLogger.LogEnd("done.");
        }

        public SeriesInfo GetSeriesInfo(long tvdbId)
        {
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(GetSeriesInfoUrl(tvdbId));

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();
            return new SeriesInfo(ZipHandler.Unzip(resStream), ENGLISH_CODE, selectedBannerMirror);
        }

        public BannerInfo GetSeasonBanner(int season, SeriesInfo seriesInfo)
        {
            try
            {
                return (from b in seriesInfo.Banners
                       where (b.BannerType == BannerType.Season) && (b.BannerSubType == BannerSubType.Season) && (b.Season == season) && (b.Language == ENGLISH_CODE)
                       orderby b.Rating descending, b.RatingCount descending 
                       select b).First();
            }
            catch (Exception)
            {

                return null;
            }
        }

        private void PopulateMirrors()
        {
            XDocument mirrorsXml = XDocument.Load(MirrorsUrl);
            var mirrors = from m in mirrorsXml.Descendants()
                            where m.Name == "Mirror"
                            select m;
            foreach (var xElement in mirrors)
            {
                string mirrorPath = string.Empty;
                if (xElement.Element("mirrorpath") != null)
                {
                    mirrorPath = xElement.Element("mirrorpath").Value;
                }
                if (string.IsNullOrWhiteSpace(mirrorPath))
                {
                    throw new Exception("Not able to find the mirror path");
                }
                int typeMask = 0;
                if (xElement.Element("typemask") != null)
                {
                    typeMask = int.Parse(xElement.Element("typemask").Value);
                }

                switch (typeMask)
                {
                    case 1: XmlMirrors.Add(mirrorPath);
                        break;
                    case 2: BannerMirrors.Add(mirrorPath);
                        break;
                    case 3: XmlMirrors.Add(mirrorPath);
                        BannerMirrors.Add(mirrorPath);
                        break;
                    case 4: ZipMirrors.Add(mirrorPath);
                        break;
                    case 5: XmlMirrors.Add(mirrorPath);
                        ZipMirrors.Add(mirrorPath);
                        break;
                    case 6: BannerMirrors.Add(mirrorPath);
                        ZipMirrors.Add(mirrorPath);
                        break;
                    case 7: XmlMirrors.Add(mirrorPath);
                        BannerMirrors.Add(mirrorPath);
                        ZipMirrors.Add(mirrorPath);
                        break;
                }
            }

            // select one of each mirror at random
            Random random = new Random();
            selectedXmlMirror = XmlMirrors.ElementAt(random.Next(XmlMirrors.Count));
            selectedBannerMirror = BannerMirrors.ElementAt(random.Next(BannerMirrors.Count)) + "/banners/";
            selectedZipMirror = ZipMirrors.ElementAt(random.Next(ZipMirrors.Count));
        }
    }
}
