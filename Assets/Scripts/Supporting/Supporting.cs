using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supporting
{

    public static void Log(string message)
    {
        Log(message, 3);
    }

    public static void Log(string message, int level)
    {
        string debugMessage = "Time: " + System.DateTime.Now;
        debugMessage += " - Frame: " + Time.frameCount;
        debugMessage += " >> " + message;

        if (level == 1)
        {
            Debug.LogError(debugMessage);
        }
        else if (level == 2)
        {
            Debug.LogWarning(debugMessage);
        }
        else
        {
            Debug.Log(debugMessage);
        }
    }

    public static bool CheckRequiredProperty(GameObject parent, GameObject toBeChecked, string label = "an object")
    {
        if (toBeChecked)
        {
            return true;
        }
        else
        {
            Log(parent.name + " - could not find " + label, 1);
            return false;
        }
    }

    public static bool CheckRequiredProperty(GameObject parent, Component toBeChecked, string label = "an object")
    {
        if (toBeChecked)
        {
            return true;
        }
        else
        {
            Log(parent.name + " - could not find " + label, 1);
            return false;
        }
    }

    public static bool CheckRequiredProperty(GameObject parent, Object toBeChecked, string label = "an object")
    {
        if (toBeChecked)
        {
            return true;
        }
        else
        {
            Log(parent.name + " - could not find " + label, 1);
            return false;
        }
    }

    public static int Modulus(int sideA, int sideB)
    {
        return sideB - sideA * Mathf.FloorToInt(sideA / sideB);
    }
}
