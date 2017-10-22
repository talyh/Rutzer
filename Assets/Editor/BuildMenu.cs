using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildMenu
{
    [MenuItem("Build/iOS/Dev")]
    public static void BuildIOSDev()
    {
        BuildIOS(true);
    }

    static void BuildIOS(bool devBuild = false)
    {
        Build(BuildTarget.iOS, devBuild);
    }

    static void Build(BuildTarget target, bool devBuild = false)
    {
        // determine relative path to help determine where to build to
        string pathToAssets = Application.dataPath;
        string pathToProject = pathToAssets.Replace("/Assets", "");
        string buildPath = string.Format("{0}/Builds/{1}{2}/InfiniteRunner{3}", pathToProject, target, devBuild ? "Dev" : "Rel", 1); // TODO append release control

        // configure the options for the build
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.locationPathName = buildPath;
        options.options = devBuild ? BuildOptions.Development : BuildOptions.None;
        options.target = target;
        options.scenes = new string[] { "Assets/Scenes/Game.unity" };

        // trigger the build process
        Debug.Log(string.Format("Building to {0}", buildPath));
        BuildPipeline.BuildPlayer(options);
        Debug.Log(string.Format("{0}{1} Build Complete", target, devBuild ? "Dev" : "Rel"));
    }
}