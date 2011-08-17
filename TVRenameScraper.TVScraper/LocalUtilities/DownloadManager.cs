using System.IO;
using System.Net;

namespace TVRenameScraper.TvScraper.LocalUtilities
{
    public static class DownloadManager
    {
        public static void DownloadAndWriteFile(string url, string writeFilePath)
        {
            if (!CustomConfiguration.DisableAllFileSystemActions)
            {
                if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(writeFilePath))
                {
                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

                    // execute the request
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    // we will read data via the response stream
                    Stream resStream = response.GetResponseStream();
                    if (resStream != null)
                    {

                        using (FileStream streamWriter = File.Create(writeFilePath))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = resStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
