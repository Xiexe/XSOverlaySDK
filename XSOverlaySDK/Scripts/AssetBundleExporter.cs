#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XSOverlaySDK
{
    public class AssetBundleExporter : EditorWindow
    {
        private static bool AssetBundleExists = false;
        private static string AssetBundleName = "xsoassetbundle";

        public static void CreateExportAssetbundle(string outputPath, UIObjects targetUI, XSOverlayThemeCreator themeCreator)
        {
            string newName = "placeholder";
            string oldName = themeCreator.ThemePreviewMaterial.name;
            switch (targetUI)
            {
                case UIObjects.WristBackground:
                    newName = "WristBackground";
                    break;

                case UIObjects.SettingsBackground:
                    newName = "SettingsBackground";
                    break;

                case UIObjects.GlobalToolbarBackground:
                    newName = "GlobalToolbarBackground";
                    break;

                case UIObjects.WindowToolbarBackground:
                    newName = "WindowToolbarBackground";
                    break;

                case UIObjects.KeyboardBackground:
                    newName = "KeyboardBackground";
                    break;
            }

            string pathToMaterial = AssetDatabase.GetAssetPath(themeCreator.ThemePreviewMaterial);
            AssetDatabase.RenameAsset(pathToMaterial, newName);
            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
            buildMap[0].assetBundleName = AssetBundleName;

            string[] materialAssets = new string[1];
            materialAssets[0] = pathToMaterial;
            buildMap[0].assetNames = materialAssets;

            BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            themeCreator.ThemePreviewMaterial.name = oldName;
            AssetDatabase.RenameAsset(pathToMaterial, oldName);
            AssetDatabase.Refresh();
        }
    }
}
#endif