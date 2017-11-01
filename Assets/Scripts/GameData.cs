using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public static class Constants
    {
        private const string STRING_KEY = "string";
        private const string INT_KEY = "int";
        private const string FLOAT_KEY = "float";

        private static Dictionary<string, object> constants = new Dictionary<string, object>();

        public static void Initialize()
        {
            Debug.Log("GameData.Constants is initializing");

            TextAsset data = Resources.Load<TextAsset>("Constants");

            string[] rows = data.text.Split('\n');

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
                        constants.Add(key, iValue);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Couldn't parse data for {0}", key));
                    }
                }
                else if (type == FLOAT_KEY)
                {
                    float fValue;
                    if (float.TryParse(value, out fValue))
                    {
                        constants.Add(key, fValue);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Couldn't parse data for {0}", key));
                    }
                }
                else if (type == STRING_KEY)
                {
                    constants.Add(key, value);
                }
                else
                {
                    Debug.LogError(string.Format("Couldn't resolve constant type for {0}", key));
                }
            }
        }

        public static T GetConstant<T>(string constantName)
        {
            return (T)constants[constantName];
        }
    }
}