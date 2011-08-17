using System.Xml.Linq;

namespace TVRenameScraper.TvScraper.Tvdb
{
    public class BannerInfo
    {
        public long ID { get; set; }
        public BannerType BannerType { get; set; }
        public BannerSubType BannerSubType { get; set; }
        public string Colors { get; set; }
        public string Language { get; set; }
        public decimal Rating { get; set; }
        public int RatingCount { get; set; }
        public bool IncludesSeriesName { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
        public string VignettePath { get; set; }
        public int? Season { get; set; }

        private const string BANNER_TYPE_FANART = "FANART";
        private const string BANNER_TYPE_POSTER = "POSTER";
        private const string BANNER_TYPE_SEASON = "SEASON";
        private const string BANNER_TYPE_SERIES = "SERIES";

        private const string BANNER_SUBTYPE_FANART_1920 = "1920X1080";
        private const string BANNER_SUBTYPE_FANART_1280 = "1280X720";
        private const string BANNER_SUBTYPE_POSTER_680 = "680X1000";
        private const string BANNER_SUBTYPE_SEASON = "SEASON";
        private const string BANNER_SUBTYPE_SEASON_WIDE = "SEASONWIDE";
        private const string BANNER_SUBTYPE_SERIES_GRAPHICAL = "GRAPHICAL";
        private const string BANNER_SUBTYPE_SERIES_BLANK = "BLANK";
        private const string BANNER_SUBTYPE_SERIES_TEXT = "TEXT";

        public BannerInfo(XElement bannerElement, string bannerMirrorPath)
        {
            ID = long.Parse(bannerElement.Element("id").Value);

            if (bannerElement.Element("BannerPath") != null && !string.IsNullOrWhiteSpace(bannerElement.Element("BannerPath").Value))
            {
                Path = bannerMirrorPath + bannerElement.Element("BannerPath").Value;
            }
            if (bannerElement.Element("Colors") != null)
            {
                Colors = bannerElement.Element("Colors").Value;
            }
            if (bannerElement.Element("Language") != null)
            {
                Language = bannerElement.Element("Language").Value;
            }
            if (bannerElement.Element("ThumbnailPath") != null && !string.IsNullOrWhiteSpace(bannerElement.Element("ThumbnailPath").Value))
            {
                ThumbnailPath = bannerMirrorPath + bannerElement.Element("ThumbnailPath").Value;
            }
            if (bannerElement.Element("VignettePath") != null && !string.IsNullOrWhiteSpace(bannerElement.Element("VignettePath").Value))
            {
                VignettePath = bannerMirrorPath + bannerElement.Element("VignettePath").Value;
            }
            if (bannerElement.Element("Rating") != null)
            {
                if (!string.IsNullOrWhiteSpace(bannerElement.Element("Rating").Value))
                {
                    Rating = decimal.Parse(bannerElement.Element("Rating").Value);
                }
            }
            if (bannerElement.Element("RatingCount") != null)
            {
                if (!string.IsNullOrWhiteSpace(bannerElement.Element("RatingCount").Value))
                {
                    RatingCount = int.Parse(bannerElement.Element("RatingCount").Value);
                }
            }
            if (bannerElement.Element("Season") != null)
            {
                if (!string.IsNullOrWhiteSpace(bannerElement.Element("Season").Value))
                {
                    Season = int.Parse(bannerElement.Element("Season").Value);
                }
            }
            if (bannerElement.Element("SeriesName") != null)
            {
                IncludesSeriesName = bool.Parse(bannerElement.Element("SeriesName").Value);
            }
            if (bannerElement.Element("BannerType") != null)
            {
                string val = bannerElement.Element("BannerType").Value.ToUpper();
                if(val == BANNER_TYPE_FANART) BannerType = BannerType.Fanart;
                if(val == BANNER_TYPE_POSTER) BannerType = BannerType.Poster;
                if(val == BANNER_TYPE_SEASON) BannerType = BannerType.Season;
                if(val == BANNER_TYPE_SERIES) BannerType = BannerType.Series;
            }
            if (bannerElement.Element("BannerType2") != null)
            {
                string val = bannerElement.Element("BannerType2").Value.ToUpper();
                if (val == BANNER_SUBTYPE_FANART_1920) BannerSubType = BannerSubType.FanartSize1920;
                if (val == BANNER_SUBTYPE_FANART_1280) BannerSubType = BannerSubType.FanartSize1280;
                if (val == BANNER_SUBTYPE_POSTER_680) BannerSubType = BannerSubType.PosterSize680;
                if (val == BANNER_SUBTYPE_SEASON) BannerSubType = BannerSubType.Season;
                if (val == BANNER_SUBTYPE_SEASON_WIDE) BannerSubType = BannerSubType.Seasonwide;
                if (val == BANNER_SUBTYPE_SERIES_GRAPHICAL) BannerSubType = BannerSubType.SeriesGraphical;
                if (val == BANNER_SUBTYPE_SERIES_BLANK) BannerSubType = BannerSubType.SeriesBlank;
                if (val == BANNER_SUBTYPE_SERIES_TEXT) BannerSubType = BannerSubType.SeriesText;
            }
        }
    }
}
