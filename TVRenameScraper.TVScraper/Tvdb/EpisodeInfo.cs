using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TVRenameScraper.TvScraper.Tvdb
{
    public class EpisodeInfo
    {
        public long ID { get; set; }
        public int SeasonNumber { get; set; }
        public int DvdEpisodeNumber { get; set; }
        public string Director { get; set; }
        public string EpisodeName { get; set; }
        public int EpisodeNumber { get; set; }
        public string FirstAired { get; set; }
        public List<ActorInfo> GuestActors { get; set; }
        public string ImdbId { get; set; }
        public string Language { get; set; }
        public string Overview { get; set; }
        public decimal Rating { get; set; }
        public int RatingCount { get; set; }
        public string Writer { get; set; }
        public string ThumbnailUrl { get; set; }

        public EpisodeInfo(XElement episodeElement, string bannerMirrorPath)
        {
            ID = long.Parse(episodeElement.Element("id").Value);
            try
            {
                SeasonNumber = int.Parse(episodeElement.Element("SeasonNumber").Value);
            }
            catch (Exception)
            {
                SeasonNumber = 0;
            }
            EpisodeNumber = int.Parse(episodeElement.Element("EpisodeNumber").Value);
            try
            {
                DvdEpisodeNumber = Convert.ToInt32(Convert.ToDecimal(episodeElement.Element("DVD_episodenumber").Value));
            }
            catch (Exception)
            {
                DvdEpisodeNumber = EpisodeNumber;
            }
            if (episodeElement.Element("Director") != null)
            {
                Director = episodeElement.Element("Director").Value ?? string.Empty;
            }
            if (episodeElement.Element("EpisodeName") != null)
            {
                EpisodeName = episodeElement.Element("EpisodeName").Value ?? string.Empty;
            }

            if (episodeElement.Element("FirstAired") != null)
            {
                FirstAired = episodeElement.Element("FirstAired").Value ?? string.Empty;
            }
            if (episodeElement.Element("IMDB_ID") != null)
            {
                ImdbId = episodeElement.Element("IMDB_ID").Value ?? string.Empty;
            }
            if (episodeElement.Element("Language") != null)
            {
                Language = episodeElement.Element("Language").Value ?? string.Empty;
            }
            if (episodeElement.Element("Overview") != null)
            {
                Overview = episodeElement.Element("Overview").Value ?? string.Empty;
            }
            if (episodeElement.Element("Writer") != null)
            {
                Writer = episodeElement.Element("Writer").Value ?? string.Empty;
            }
            if (episodeElement.Element("filename") != null && !string.IsNullOrWhiteSpace(episodeElement.Element("filename").Value))
            {
                ThumbnailUrl = bannerMirrorPath + episodeElement.Element("filename").Value ?? string.Empty;
            }
            try
            {
                Rating = decimal.Parse(episodeElement.Element("Rating").Value);
            }
            catch (Exception)
            {
                Rating = 0m;
            }
            try
            {
                RatingCount = int.Parse(episodeElement.Element("RatingCount").Value);
            }
            catch (Exception)
            {
                RatingCount = 0;
            }
            GuestActors = new List<ActorInfo>();
            if (episodeElement.Element("GuestStars") != null)
            {
                string guestActors = episodeElement.Element("GuestStars").Value;
                string[] actors = guestActors.Split('|');
                foreach (var actor in actors)
                {
                    GuestActors.Add(new ActorInfo { Name = actor });
                }
            }
        }
    }
}
