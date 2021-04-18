#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace XSOverlaySDK
{
    /*
         Xiexe -
         This exists solely because Unity UI is fucking stupid and I couldn't
         figure out a way to force the material on the Image component to actually update
         without selecting the object in some way first. Setting dirty, forcing the canvas
         to update, etc, all did not work. This is because the Editor for the Image component
         needs to be in view to actually update. Custom editors also have this limitation.

         So instead, I've lost my mind, and created a second editor script that gets a flag set,
         and then the selected object is set to the Image, which updates this Editor, and the Image,
         and since the flag was set on this Editor, it then disables the flag and sets the selected
         object back to the Root object.

         I hate Unity UI. Fuck.
    */

    public class RefreshImage : MonoBehaviour
    {
        public GameObject RootEditor;
        public bool NeedsRefresh = false;
    }

    [CustomEditor(typeof(RefreshImage))]
    public class RefreshImageEditor : Editor
    {
        private RefreshImage _RefreshImage;

        private void OnEnable()
        {
            _RefreshImage = (RefreshImage)target;
        }

        public override void OnInspectorGUI()
        {
            _RefreshImage.RootEditor = (GameObject)EditorGUILayout.ObjectField("Root Editor", _RefreshImage.RootEditor, typeof(GameObject), true);

            if (_RefreshImage.NeedsRefresh)
            {
                _RefreshImage.NeedsRefresh = false;
                Selection.activeObject = _RefreshImage.RootEditor;
            }
        }
    }
}
#endif