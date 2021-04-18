using UnityEditor;
using UnityEngine;

public class SDKImportInitializer : Editor
{
    [InitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        if (PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            PlayerSettings.colorSpace = ColorSpace.Linear;
        }
    }
}
