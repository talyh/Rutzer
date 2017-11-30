using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Downloader
    {
        private const string SPREADSHEET_ID = "1zpfxKZx3y7akX3zFkdRHeWvF8a6t8qAnOVySeYE2z_o"; //after the d/
        private const string LOCALIZATION_TAB_ID = "0"; //gid
        private const string CONSTANTS_TAB_ID = "1136041927";
        private const string COIN_PACKS_TAB_ID = "502225398";
        private const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&id={0}&gid={1}";

        public static void DonwloadLocalization()
        {
            DownloadCSV(LOCALIZATION_TAB_ID, Localization.filePath);
        }

        public static void DownloadConstants()
        {
            DownloadCSV(CONSTANTS_TAB_ID, GameData.Constants.filePath);
        }

        public static void DownloadCoinPacksData()
        {
            DownloadCSV(COIN_PACKS_TAB_ID, GameData.CoinPacksDataManager.filePath);
        }

        private static void DownloadCSV(string tabID, string filePath)
        {
            //Get the formatted URL
            string downloadUrl = string.Format(URL_FORMAT, SPREADSHEET_ID, tabID);

            Supporting.Log(string.Format("Downloading {0} to {1}", tabID, filePath));

            //Download the data
            WWW website = new WWW(downloadUrl);

            //Wait for data to download
            while (!website.isDone)
            {
            }

            if (!string.IsNullOrEmpty(website.error))
            {
                Supporting.Log(website.error, 1);

                // cached values are automatically pulled by the clients, based on what was last writen to the filepath
                Supporting.Log("Using cached values instead");
            }
            else
            {
                //Successfully got the data, process it
                //Save to disk
                File.WriteAllText(filePath, website.text);
            }
        }
    }
}
