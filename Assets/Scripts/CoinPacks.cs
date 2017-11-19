using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class CoinPacksData
    {
        private static Dictionary<string, CoinPack> _coinPacks;

        public static string filePath
        {
            get
            {
                return string.Format("{0}/CoinPacksData.csv", Application.persistentDataPath);
            }
        }

        public static void Initialize()
        {
            // initialize Dictionary here to be sure it's initialized when we need it to
            _coinPacks = new Dictionary<string, CoinPack>();

            // fetch the data
            GameData.Downloader.DownloadCoinPacksData();

            //Now load using System.IO File.
            StreamReader csv = File.OpenText(filePath);

            // break the csv into rows
            string[] rows = csv.ReadToEnd().Split('\n');

            // loop through the rows, creating a new CoinPack for each and adding it to the Dictionary
            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');

                string key = columns[0].Trim().ToUpper();
                int coins;
                bool coinsPacse = int.TryParse(columns[1].Trim(), out coins);
                int relativeProbability;
                bool relativeProbabilityParse = int.TryParse(columns[2].Trim(), out relativeProbability);

                if (coinsPacse && relativeProbabilityParse)
                {
                    CoinPack coinPack = new CoinPack(coins, relativeProbability);
                    _coinPacks.Add(key, coinPack);
                }
            }
        }

        //Get the localized string
        public static CoinPack Get(string key)
        {
            if (_coinPacks == null)
            {
                Initialize();
            }

            return _coinPacks[key.ToUpper()];
        }

        public static bool CheckKey(string key)
        {
            if (_coinPacks == null)
            {
                Initialize();
            }

            return _coinPacks.ContainsKey(key.ToUpper());
        }
    }
}

public class CoinPack
{
    private int _coins;
    public int coins
    {
        get { return _coins; }
    }

    private int _relativeProbability;
    public int relativeProbability
    {
        get { return _relativeProbability; }
    }

    public CoinPack(int coins, int relativeProbability)
    {
        _coins = coins;
        _relativeProbability = relativeProbability;
    }
}