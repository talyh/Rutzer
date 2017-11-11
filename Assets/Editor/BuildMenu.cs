using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BuildMenu
{
    // Build for iOS 
    [MenuItem("Build/iOS/Dev")]
    public static void BuildIOSDev()
    {
        BuildIOS(true);
    }

    [MenuItem("Build/iOS/Rel")]
    public static void BuildIOSRel()
    {
        BuildIOS();
    }

    [MenuItem("Build/iOS/All")]
    public static void BuildIOSAll()
    {
        BuildIOS();
        BuildIOS(true);
    }

    static void BuildIOS(bool devBuild = false)
    {
        Build(BuildTarget.iOS, devBuild);
    }

    // Build for Android
    [MenuItem("Build/Android/Dev")]
    static void BuildAndroidDev()
    {
        BuildAndroid(true);
    }

    [MenuItem("Build/Android/Rel")]
    static void BuildAndroidRel()
    {
        BuildAndroid();
    }

    [MenuItem("Build/Android/All")]
    public static void BuildAndroidAll()
    {
        BuildAndroid();
        BuildAndroid(true);
    }

    static void BuildAndroid(bool devBuild = false)
    {
        Build(BuildTarget.Android, devBuild);
    }


    // Build for All Target Platforms
    [MenuItem("Build/All")]
    public static void BuildAll()
    {
        BuildIOSAll();
        BuildAndroidAll();
    }

    static void Build(BuildTarget target, bool devBuild = false)
    {
        // determine relative path to help determine where to build to
        string pathToAssets = Application.dataPath;
        string pathToProject = pathToAssets.Replace("/Assets", "");
        string currentTime = System.DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss");
        string buildPath = string.Format("{0}/Builds/InfiniteRunner_{1}_{2}_{3}", pathToProject, target, devBuild ? "Dev" : "Rel", currentTime);

        // configure the options for the build
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.locationPathName = buildPath;
        options.options = devBuild ? BuildOptions.Development : BuildOptions.None;
        options.target = target;
        options.scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            options.scenes[i] = EditorBuildSettings.scenes[i].path;
            Debug.Log(options.scenes[i]);
        }

        // trigger the build process
        Debug.Log(string.Format("Building to {0}", buildPath));
        BuildPipeline.BuildPlayer(options);
        Debug.Log(string.Format("{0}{1} Build Complete", target, devBuild ? "Dev" : "Rel"));
    }
}