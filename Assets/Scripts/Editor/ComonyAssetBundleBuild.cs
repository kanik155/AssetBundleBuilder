using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using ComonyAPI.GCP;

public class ComonyAssetBundleBuild
{
    public static void Build(string scenePath)
    {
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        ComonySDK sdk = GameObject.Find("ComonySDK").GetComponent<ComonySDK>();
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        if(sdk.SpaceUUID == null)
        {
            buildMap[0].assetBundleName = "space_" + GetUUID() + ".visiolion";
        }
        else
        {
            buildMap[0].assetBundleName = "space_" + sdk.SpaceUUID + ".visiolion";
        }

        string[] scenes = new string[1];
        string tempScenePath = Directory.GetParent(scenePath) + "/" + buildMap[0].assetBundleName + ".unity";
        AssetDatabase.CopyAsset(scenePath, tempScenePath);

        scenes[0] = tempScenePath;
        buildMap[0].assetNames = scenes;

        //string assetBundleDirWin = "Assets/AssetBundles/space/win";
        string assetBundleDirMac = "Assets/AssetBundles/space/mac";
        //string assetBundleDirAndroid = "Assets/AssetBundles/space/android";
        //string assetBundleDirIos = "Assets/AssetBundles/space/ios";
        /*
        if (!Directory.Exists(assetBundleDirWin))
        {
            Directory.CreateDirectory(assetBundleDirWin);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirWin, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        */

        if (!Directory.Exists(assetBundleDirMac))
        {
            Directory.CreateDirectory(assetBundleDirMac);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirMac, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);

        /*
        if (!Directory.Exists(assetBundleDirAndroid))
        {
            Directory.CreateDirectory(assetBundleDirAndroid);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirAndroid, buildMap, BuildAssetBundleOptions.None, BuildTarget.Android);
        if (!Directory.Exists(assetBundleDirIos))
        {
            Directory.CreateDirectory(assetBundleDirIos);
        }

        BuildPipeline.BuildAssetBundles(assetBundleDirIos, buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);
        */

        AssetDatabase.DeleteAsset(tempScenePath);

        var path = Application.dataPath + "/AssetBundles";
        var ignores = new List<string>();
        // Storage.Upload(path, "space/win", ignores.Count == 0 ? null : ignores.ToArray());

        // Directory.Delete(Application.dataPath + "/AssetBundles", true);
    }

    private static string GetUUID()
    {
        var guid = System.Guid.NewGuid();
        return guid.ToString();
    }
}
