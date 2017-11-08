﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    private const string DEFAULT_LANGUAGE = "ENGLISH";

    static Dictionary<string, string> localizations;

    public static void Initialize()
    {
        // initialize Dictionary here to be sure it's initialized when we need it to
        localizations = new Dictionary<string, string>();

        //Load the CSV
        TextAsset csv = Resources.Load<TextAsset>("Localizations");

        //Get the system language
        SystemLanguage language = Application.systemLanguage;

        // initiliaze index to help find the csv column for the current language
        int currentLanguageIndex = -1;

        // break the csv into rows
        string[] rows = csv.text.Split('\n');

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

            localizations.Add(key, value);
        }
    }

    //Get the localized string
    public static string Get(string key)
    {
        //Make sure the CSV has been loaded
        if (localizations == null)
        {
            Initialize();
        }

        return localizations[key.ToUpper()];

    }

    //A fancy way to get the localization string, not necessary 
    public static string Localize(this string key)
    {
        return Get(key);
    }
}