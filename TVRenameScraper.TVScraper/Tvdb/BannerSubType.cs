namespace TVRenameScraper.TvScraper.Tvdb
{
    public enum BannerSubType
    {
        None,
        /// <summary>
        /// Fanart - indicates the size as 1920x1280
        /// </summary>
        FanartSize1920,
        /// <summary>
        /// Fanart - indicates the size as 1280x720
        /// </summary>
        FanartSize1280,
        /// <summary>
        /// Poster - indicates the size as 680x1000
        /// </summary>
        PosterSize680,
        /// <summary>
        /// Season - Standard DVD cover size
        /// </summary>
        Season,
        /// <summary>
        /// Season - Wide banner format
        /// </summary>
        Seasonwide,
        /// <summary>
        /// Series - the artwork displays the show name or logo
        /// </summary>
        SeriesGraphical,
        /// <summary>
        /// Series - artwork with no text or logo on it
        /// </summary>
        SeriesBlank,
        /// <summary>
        /// Artwork with the show name written on it in plain arial font
        /// </summary>
        SeriesText
    }
}
