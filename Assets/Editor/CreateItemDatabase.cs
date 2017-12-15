using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateItemDatabase
{
    // NOT REALLY PART OF THE GAME, JUST REFERENCE FOR THE FUTURE
    const string PATH = "Data/ItemDatabase";

    [MenuItem("Data/ItemDatabase/CreateORFind")]
    public static void Create()
    {
        ItemDatabase itemDatabase = Resources.Load<ItemDatabase>(PATH);

        if (itemDatabase == null)
        {
            itemDatabase = ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(itemDatabase, string.Format("Assets//Resources/{0}.asset", PATH));
            AssetDatabase.SaveAssets();
        }

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = itemDatabase;
    }
}
