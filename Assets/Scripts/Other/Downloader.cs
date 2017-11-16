using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Downloader
    {
        //Get these from your own URL
        const string SPREADSHEET_ID = "1zpfxKZx3y7akX3zFkdRHeWvF8a6t8qAnOVySeYE2z_o"; //after the d/
        const string LOCALIZATION_TAB_ID = "0"; //gid
        const string CONSTANTS_TAB_ID = "1136041927";
        const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&id={0}&gid={1}";

        //Get url by using File->Download As->CSV
        //Then open download history in chrome/firefox and copy url
        //Make sure accessiblity is set to Anyone with link can view

        public static void Initialize()
        {
            DownloadCSV(LOCALIZATION_TAB_ID, Localization.filePath);
            // TODO - fix
            DownloadCSV(CONSTANTS_TAB_ID, GameData.Constants.filePath);
        }

        private static void DownloadCSV(string tabID, string filePath)
        {
            //Get the formatted URL
            string downloadUrl = string.Format(URL_FORMAT, SPREADSHEET_ID, tabID);

            // Supporting.Log(string.Format("Downloading {0}", tabID));

            //Download the data
            WWW website = new WWW(downloadUrl);

            //Wait for data to download
            while (!website.isDone)
            {
            }

            if (string.IsNullOrEmpty(website.text))
            {
                Supporting.Log("NO DATA WAS RECEIVED", 1);

                //Load the last cached values
                // File.ReadAllText(filePath);
            }
            else
            {
                // Supporting.Log(website.text);

                //Successfully got the data, process it
                //Save to disk
                File.WriteAllText(filePath, website.text);
            }
        }
    }
}
