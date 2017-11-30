using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace GameData
{
    public static class CoinPacksDataManager
    {
        private static Dictionary<string, CoinPackData> _coinPacks;

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
            _coinPacks = new Dictionary<string, CoinPackData>();

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
                int lifetime;
                bool lifetimeParse = int.TryParse(columns[2].Trim(), out lifetime);

                if (coinsPacse && lifetimeParse)
                {
                    CoinPackData coinPack = new CoinPackData(coins, lifetime);
                    _coinPacks.Add(key, coinPack);
                }
            }
        }

        public static CoinPackData Get(string key)
        {
            if (_coinPacks == null)
            {
                Initialize();
            }

            return _coinPacks[key.ToUpper()];
        }

        public static CoinPackData GetRandom()
        {
            if (_coinPacks == null)
            {
                Initialize();
            }

            int selected = Random.Range(0, _coinPacks.Count);
            string[] keys = _coinPacks.Keys.ToArray();
            string entry = "";

            for (int i = 0; i < keys.Length; i++)
            {
                if (i == selected)
                {
                    entry = keys[i];
                    break;
                }
            }

            return _coinPacks[entry];
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


    public class CoinPackData
    {
        private int _coins;
        public int coins
        {
            get { return _coins; }
        }

        private int _lifetime;
        public int lifetime
        {
            get { return _lifetime; }
        }

        public CoinPackData(int coins, int lifetime)
        {
            _coins = coins;
            _lifetime = lifetime;
        }

        public override string ToString()
        {
            return string.Format("coins: {0}, lifetime: {1}", _coins, _lifetime);
        }
    }
}