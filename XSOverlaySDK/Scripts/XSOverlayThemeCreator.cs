#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace XSOverlaySDK
{
    public enum UIObjects
    {
        WristBackground,
        SettingsBackground,
        GlobalToolbarBackground,
        WindowToolbarBackground,
        KeyboardBackground
    }

    public class XSOverlayThemeCreator : MonoBehaviour
    {

        public Material ThemePreviewMaterial;
        public Image ThemePreviewImg;
        public RefreshImage ImageRefresher;
    }

    //Inspector
    [CustomEditor(typeof(XSOverlayThemeCreator))]
    public class WristThemeEditor : Editor
    {
        private XSOverlayThemeCreator ThemeCreator;
        private RefreshImage ImageRefresher;
        public UIObjects TargetWindow;
        private static string ThemeName;
        private bool InvalidName = false;

        private void OnEnable()
        {
            ThemeCreator = (XSOverlayThemeCreator)target;
        }

        public override void OnInspectorGUI()
        {
            TargetWindow = (UIObjects)EditorGUILayout.EnumPopup("UI To Theme", TargetWindow);
            ThemeCreator.ThemePreviewMaterial = (Material)EditorGUILayout.ObjectField("Theme Material", ThemeCreator.ThemePreviewMaterial, typeof(Material), false);
            ThemeName = EditorGUILayout.TextField("Theme Name", ThemeName);

            if (ThemeCreator.ThemePreviewMaterial == null)
                EditorGUILayout.HelpBox("Please assign a preview material with a UI Shader of your choosing!", MessageType.Error);

            string uiName = Enum.GetName(typeof(UIObjects), TargetWindow);
            switch (TargetWindow)
            {
                case UIObjects.WristBackground:
                    if (ThemeCreator.ThemePreviewImg != null && ThemeCreator.ThemePreviewMaterial != null)
                        DrawButtons();
                    break;

                case UIObjects.SettingsBackground:
                    EditorGUILayout.HelpBox($"{uiName} cannot be themed currently, but is PLANNED for future releases!", MessageType.Error);
                    break;

                case UIObjects.GlobalToolbarBackground:
                    EditorGUILayout.HelpBox($"{uiName} cannot be themed currently, but is PLANNED for future releases!", MessageType.Error);
                    break;

                case UIObjects.WindowToolbarBackground:
                    EditorGUILayout.HelpBox($"{uiName} cannot be themed currently, but is PLANNED for future releases!", MessageType.Error);
                    break;

                case UIObjects.KeyboardBackground:
                    EditorGUILayout.HelpBox($"{uiName} cannot be themed currently, but is PLANNED for future releases!", MessageType.Error);
                    break;
            }

            if(InvalidName)
                EditorGUILayout.HelpBox("Please name your theme, and try again!", MessageType.Error);

            EditorGUILayout.HelpBox("NOTE: Themes cannot currently move UI elements. The default user icon is only there to help you with alignment.", MessageType.Info);
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Preview"))
            {
                ImageRefresher = ThemeCreator.gameObject.GetComponentInChildren<RefreshImage>();
                if (ImageRefresher != null)
                {
                    ThemeCreator.ThemePreviewImg = ImageRefresher.GetComponent<Image>();
                    ThemeCreator.ImageRefresher = ImageRefresher;

                    Debug.Log("Update Preview Material: You may need to hit Ctrl+S to see changes!");
                }

                ThemeCreator.ThemePreviewImg.material = ThemeCreator.ThemePreviewMaterial;
                ThemeCreator.ThemePreviewImg.SetAllDirty();
                ThemeCreator.ImageRefresher.NeedsRefresh = true;
                Selection.activeObject = ThemeCreator.ThemePreviewImg.gameObject;
            }

            if (GUILayout.Button("Build"))
            {
                if (String.IsNullOrEmpty(ThemeName))
                {
                    InvalidName = true;
                    Debug.Log("Cannot build theme, invalid name!");
                }
                else
                {
                    InvalidName = false;
                    CheckForExportedThemeFolderThenBuild(TargetWindow);
                    Debug.Log($"Built theme: {ThemeName}");
                }
            }
        }

        private void CheckForExportedThemeFolderThenBuild(UIObjects targetUI)
        {
            string uiName = Enum.GetName(typeof(UIObjects), targetUI);
            if (!AssetDatabase.IsValidFolder("Assets/ExportedThemes"))
            {
                AssetDatabase.CreateFolder("Assets", "ExportedThemes");
                Debug.Log("Created export folder for themes!");
            }

            if (!AssetDatabase.IsValidFolder($"Assets/ExportedThemes/{uiName}"))
            {
                AssetDatabase.CreateFolder("Assets/ExportedThemes", uiName);
                Debug.Log($"Create export folder for {uiName}");
            }

            if (!AssetDatabase.IsValidFolder($"Assets/ExportedThemes/{uiName}/{ThemeName}"))
            {
                AssetDatabase.CreateFolder($"Assets/ExportedThemes/{uiName}", ThemeName);
                Debug.Log($"Created export folder for theme: {ThemeName}");
            }

            string fullPathToTheme = $"{Application.dataPath}/ExportedThemes/{uiName}/{ThemeName}";
            AssetBundleExporter.CreateExportAssetbundle(fullPathToTheme, targetUI, ThemeCreator);
        }
    }
}
#endif
