using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Constants
    {
        private const string STRING_KEY = "string";
        private const string INT_KEY = "int";
        private const string FLOAT_KEY = "float";

        public enum constantKeywords { INITIAL_SPEED, MAX_SPEED, HUD_SPEED_MULTIPLIER, POINTS_MULTIPLIER, POINTS_FOR_SPEED_INCREASE }


        public static string filePath
        {
            get
            {
                return string.Format("{0}/Constants.csv", Application.persistentDataPath);
            }
        }

        private static Dictionary<string, object> _constants;

        public static void Initialize()
        {
            // Supporting.Log("GameData.Constants is initializing");

            _constants = new Dictionary<string, object>();

            // fetch the data
            GameData.Downloader.DownloadConstants();

            //Now load using System.IO File.
            StreamReader csv = File.OpenText(filePath);

            // break the csv into rows
            string[] rows = csv.ReadToEnd().Split('\n');

            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');

                string key = columns[0].Trim();
                string type = columns[1].Trim();
                string value = columns[2].Trim();

                if (type == INT_KEY)
                {
                    int iValue;
                    if (int.TryParse(value, out iValue))
                    {
                        _constants.Add(key, iValue);
                    }
                    else
                    {
                        Supporting.Log(string.Format("Couldn't parse data for {0}", key), 1);
                    }
                }
                else if (type == FLOAT_KEY)
                {
                    float fValue;
                    if (float.TryParse(value, out fValue))
                    {
                        _constants.Add(key, fValue);
                    }
                    else
                    {
                        Supporting.Log(string.Format("Couldn't parse data for {0}", key), 1);
                    }
                }
                else if (type == STRING_KEY)
                {
                    _constants.Add(key, value);
                }
                else
                {
                    Supporting.Log(string.Format("Couldn't resolve constant type for {0}", key), 1);
                }
            }
        }

        public static T GetConstant<T>(string constantName)
        {
            if (_constants == null)
            {
                Initialize();
            }

            return (T)_constants[constantName];
        }
    }
}