using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public static class Constants
    {
        // private static Dictionary<string, int> int_constants = new Dictionary<string, int>();
        // private static Dictionary<string, float> float_constants = new Dictionary<string, float>();
        // private static Dictionary<string, string> string_constants = new Dictionary<string, string>();

        private static Dictionary<string, object> constants = new Dictionary<string, object>();

        public static void Initialize()
        {
            Debug.Log("GameData.Constants is initializing");

            TextAsset data = Resources.Load<TextAsset>("Constants");

            string[] rows = data.text.Split('\n');

            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');

                string key = columns[0];
                string type = columns[1];
                string value = columns[2];

                if (type == "int")
                {
                    int iValue;
                    if (int.TryParse(value, out iValue))
                    {
                        // int_constants.Add(key, iValue);
                        constants.Add(key, iValue);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Couldn't parse data for {0}", key));
                    }
                }
                else if (type == "float")
                {
                    float fValue;
                    if (float.TryParse(value, out fValue))
                    {
                        // float_constants.Add(key, fValue);
                        constants.Add(key, fValue);
                    }
                    else
                    {
                        Debug.LogError(string.Format("Couldn't parse data for {0}", key));
                    }
                }
                else if (type == "string")
                {
                    // string_constants.Add(key, value);
                    constants.Add(key, value);
                }
                else
                {
                    Debug.LogError("Couldn't resolve constant type");
                }
            }

            // Debug.Log("INTS");
            // foreach (KeyValuePair<string, int> value in int_constants)
            // {
            //     Debug.Log(value);
            // }
            // Debug.Log("FLOATS");
            // foreach (KeyValuePair<string, float> value in float_constants)
            // {
            //     Debug.Log(value);
            // }
            // Debug.Log("STRINGS");
            // foreach (KeyValuePair<string, string> value in string_constants)
            // {
            //     Debug.Log(value);
            // }
            // Debug.Log("CONSTANTS");
            // foreach (KeyValuePair<string, object> value in constants)
            // {
            //     Debug.Log(value);
            // }
        }

        public static T GetConstant<T>(string constantName)
        {
            return (T)constants[constantName];
        }
    }
}