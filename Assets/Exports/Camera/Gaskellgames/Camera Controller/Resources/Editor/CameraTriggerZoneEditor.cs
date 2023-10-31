#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

using Gaskellgames;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(CameraTriggerZone))] [CanEditMultipleObjects]
    public class CameraTriggerZoneEditor : Editor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty cameraRig;
        SerializedProperty alwaysShowZone;
        SerializedProperty triggerColour;
        SerializedProperty triggerOutlineColour;

        bool InfoGroup = false;

        private void OnEnable()
        {
            cameraRig = serializedObject.FindProperty("cameraRig");
            alwaysShowZone = serializedObject.FindProperty("alwaysShowZone");
            triggerColour = serializedObject.FindProperty("triggerColour");
            triggerOutlineColour = serializedObject.FindProperty("triggerOutlineColour");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            CameraTriggerZone CcmeraTriggerZone = (CameraTriggerZone)target;
            serializedObject.Update();

            /*
            // draw default inspector
            base.OnInspectorGUI();
            */

            // banner
            Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Gaskellgames/Camera Controller/Resources/Icons/inspectorBanner_CameraController.png", typeof(Texture));
            GUILayout.Box(banner, GUILayout.ExpandWidth(true), GUILayout.Height(Screen.width / 7.5f));

            // custom inspector
            EditorGUILayout.PropertyField(alwaysShowZone);
            EditorGUILayout.PropertyField(cameraRig);
            EditorGUILayout.Space();

            InfoGroup = EditorGUILayout.BeginFoldoutHeaderGroup(InfoGroup, "Sensor Info");
            if (InfoGroup)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(triggerColour);
                EditorGUILayout.PropertyField(triggerOutlineColour);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}

#endif
