using System.Collections;
using System.ComponentModel;
using System.Configuration;
using TVRenameScraper.TvScraper.LocalUtilities;


namespace TVRenameScraper.TvScraper.Installer
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        private const int DEFAULT_DELAY = 500;
        private const int DEFAULT_SERIES_DELAY = 0;
        private const bool DEFAULT_DISABLE = false;

        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string targetDirectory = Context.Parameters["targetdir"];

            string tvRenamePath = Context.Parameters["TVRENAMEPATH"];

            //System.Diagnostics.Debugger.Break();

            string exePath = string.Format("{0}TvRenameScraper.exe", targetDirectory);

            Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            config.AppSettings.Settings["TVRenamePath"].Value = tvRenamePath;
            config.AppSettings.Settings["DisableAllActions"].Value = DEFAULT_DISABLE.ToString();
            config.AppSettings.Settings["PostCompletionDelay"].Value = DEFAULT_DELAY.ToString();
            config.AppSettings.Settings["PostSeriesDelay"].Value = DEFAULT_SERIES_DELAY.ToString();

            config.Save();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            XmlCache.RemoveCache();
        }
    }
}
