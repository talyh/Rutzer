using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Localization
{
    private const string DEFAULT_LANGUAGE = "ENGLISH";

    static Dictionary<string, string> _localizations;

    public static string filePath
    {
        get
        {
            string persistentPath = Application.persistentDataPath;
            string savePath = string.Format("{0}/Localization.csv", persistentPath);

            return savePath;
        }
    }

    public static void Initialize()
    {
        // initialize Dictionary here to be sure it's initialized when we need it to
        _localizations = new Dictionary<string, string>();

        // fetch the data
        GameData.Downloader.Initialize();

        //Now load using System.IO File.
        StreamReader csv = File.OpenText(filePath);

        //Get the system language
        SystemLanguage language = Application.systemLanguage;

        // initiliaze index to help find the csv column for the current language
        int currentLanguageIndex = -1;

        // break the csv into rows
        string[] rows = csv.ReadToEnd().Split('\n');

        // use the first row headers, starting in column 2, to identify the languages
        string[] languageIndex = rows[0].Split(',');

        for (int i = 1; i < languageIndex.Length; i++)
        {
            //Get the language index
            if (language.ToString().ToUpper() == languageIndex[i].Trim().ToUpper())
            {
                currentLanguageIndex = i;
            }
        }

        // use default language if system language wasn't found
        if (currentLanguageIndex == -1)
        {
            currentLanguageIndex = System.Array.IndexOf(languageIndex, DEFAULT_LANGUAGE.ToUpper());
        }

        for (int i = 1; i < rows.Length; i++)
        {
            //Parse the csv into the dictionary
            string[] columns = rows[i].Split(',');

            string key = columns[0].Trim().ToUpper();
            string value = columns[currentLanguageIndex].Trim();

            _localizations.Add(key, value);
        }
    }

    //Get the localized string
    public static string Get(string key)
    {
        //Make sure the CSV has been loaded
        if (_localizations == null)
        {
            Initialize();
        }

        return _localizations[key.ToUpper()];

    }

    //A fancy way to get the localization string, not necessary 
    public static string Localize(this string key)
    {
        return Get(key);
    }
}