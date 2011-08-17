using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TVRenameScraper.TvScraper.Tvdb
{
    public class SeriesInfo
    {
        private XDocument LanguageXml { get; set; }

        private XDocument BannersXml { get; set; }

        private XDocument ActorsXml { get; set; }

        #region properties of a series

        public string Title { get; set; }

        public long ID { get; set; }

        public string Rating { get; set; }

        public string MpaaClassification { get; set; }

        public string DatePremiered { get; set; }

        public string Studio { get; set; }

        public string Plot { get; set; }

        public List<ActorInfo> Actors { get; set; }

        public string FanartUrl { get; set; }

        public string PosterUrl { get; set; }

        public List<BannerInfo> Banners { get; set; }

        public List<EpisodeInfo> Episodes { get; set; }

        #endregion

        public SeriesInfo(List<KeyValuePair<string, XDocument>> xmlDocuments, string languageCode, string bannersMirrorUrl)
        {
            foreach (var keyValuePair in xmlDocuments)
            {
                if (keyValuePair.Key == languageCode + ".xml")
                {
                    LanguageXml = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "banners.xml")
                {
                    BannersXml = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "actors.xml")
                {
                    ActorsXml = keyValuePair.Value;
                }
                // ignore other documents
            }
            if (LanguageXml == null || BannersXml == null || ActorsXml == null)
            {
                // is this step needed? Do all shows have this info?
                throw new Exception("Series Info is missing xml files");
            }
            
            // process the series XML info
            GetInfoFromLanguageXml(bannersMirrorUrl);
            GetInfoFromActorsXml(bannersMirrorUrl);

            // process the banners XML info
            GetInfoFromBannersXml(bannersMirrorUrl);
        }

        private void GetInfoFromActorsXml(string bannersMirrorUrl)
        {
            Actors = new List<ActorInfo>();
            XElement actorsRootElement = ActorsXml.Element("Actors");
            if (actorsRootElement == null) throw new Exception("Could not find actors root element in Actors XML");
            var actorElements = actorsRootElement.Elements("Actor");
            foreach (var actorElement in actorElements)
            {
                ActorInfo actorInfo = new ActorInfo();
                actorInfo.Name = actorElement.Element("Name").Value;
                actorInfo.Role = actorElement.Element("Role").Value;
                actorInfo.ThumbnailUrl = bannersMirrorUrl + actorElement.Element("Image").Value;
                Actors.Add(actorInfo);
            }
        }

        private void GetInfoFromLanguageXml(string bannersMirrorUrl)
        {
            XElement dataElement = LanguageXml.Element("Data");
            if(dataElement == null) throw new Exception("Could not find data element in Language XML");
            XElement seriesElement = dataElement.Element("Series");
            if (seriesElement == null) throw new Exception("Could not find series element in Language XML");
            ID = long.Parse(seriesElement.Element("id").Value);
            Title = seriesElement.Element("SeriesName").Value;
            if (seriesElement.Element("Rating") != null)
            {
                Rating = seriesElement.Element("Rating").Value;
            }
            if (seriesElement.Element("ContentRating") != null)
            {
                MpaaClassification = seriesElement.Element("ContentRating").Value;
            }
            if (seriesElement.Element("FirstAired") != null)
            {
                DatePremiered = seriesElement.Element("FirstAired").Value;
            }
            if (seriesElement.Element("Network") != null)
            {
                Studio = seriesElement.Element("Network").Value;
            }
            if (seriesElement.Element("Overview") != null)
            {
                Plot = seriesElement.Element("Overview").Value;
            }
            if (seriesElement.Element("fanart") != null && !string.IsNullOrWhiteSpace(seriesElement.Element("fanart").Value))
            {
                FanartUrl = bannersMirrorUrl + seriesElement.Element("fanart").Value;
            }
            if (seriesElement.Element("poster") != null && !string.IsNullOrWhiteSpace(seriesElement.Element("poster").Value))
            {
                PosterUrl = bannersMirrorUrl + seriesElement.Element("poster").Value;
            }

            // deal with episodes
            IEnumerable<XElement> episodeElements = dataElement.Elements("Episode");
            Episodes = new List<EpisodeInfo>();
            foreach (var episodeElement in episodeElements)
            {
                EpisodeInfo episodeInfo = new EpisodeInfo(episodeElement, bannersMirrorUrl);
                Episodes.Add(episodeInfo);
            }
        }

        private void GetInfoFromBannersXml(string bannersMirrorUrl)
        {
            Banners = new List<BannerInfo>();
            XElement dataElement = BannersXml.Element("Banners");
            if (dataElement == null) throw new Exception("Could not find banners element in Banners XML");
            foreach (var bannerElement in dataElement.Elements("Banner"))
            {
                BannerInfo bannerInfo = new BannerInfo(bannerElement, bannersMirrorUrl);
                Banners.Add(bannerInfo);
            }
        }

        public EpisodeInfo GetEpisodeInfo(int seasonNumber, int episodeNumber, bool useDvdOrder)
        {
            try
            {
                return (from e in Episodes
                        where
                            e.SeasonNumber == seasonNumber &&
                            ((useDvdOrder && e.DvdEpisodeNumber == episodeNumber) ||
                             (!useDvdOrder && e.EpisodeNumber == episodeNumber))
                        select e).First();
            } catch
            {
                return null;
            }
        }
    }
}
