using UnityEngine;
using UnityEditor;

#if UNITY_STANDALONE_WIN && UNITY_64
using UnityEngine.NVIDIA;
#endif

#if DLSS_INSTALLED
using NVIDIA = UnityEngine.NVIDIA;
#endif

namespace TND.DLSS
{
    /// <summary>
    /// Base editor for DLSS components.
    /// Provides common inspector UI and functionality for both URP and HDRP implementations.
    /// </summary>
    [CustomEditor(typeof(DLSS_UTILS), editorForChildClasses: true)]
    public class DLSS_Editor : Editor
    {
        protected SerializedProperty dlssQualityProp;
        protected SerializedProperty antiGhostingProp;
        protected SerializedProperty sharpeningProp;
        protected SerializedProperty sharpnessProp;

        protected virtual void OnEnable()
        {
            dlssQualityProp = serializedObject.FindProperty("DLSSQuality");
            antiGhostingProp = serializedObject.FindProperty("m_antiGhosting");
            sharpeningProp = serializedObject.FindProperty("sharpening");
            sharpnessProp = serializedObject.FindProperty("sharpness");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if !DLSS_INSTALLED
            DrawMissingPackageWarning();
#elif !TND_HDRP_EDITEDSOURCE && UNITY_HDRP
            DrawHDRPSourceEditWarning();
#elif !UNITY_STANDALONE_WIN || !UNITY_64
            DrawPlatformNotSupportedWarning();
#elif TND_DLSS
            DrawDLSSSettings();
            DrawImageQualitySettings();
            DrawPipelineSpecificNotes();
#endif

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawMissingPackageWarning()
        {
#if UNITY_URP || UNITY_HDRP
            EditorGUILayout.HelpBox("Missing NVIDIA DLSS Package. Please install to enable DLSS features.", MessageType.Error);
            if(GUILayout.Button("Install Package")) {
                UnityEditor.PackageManager.Client.Add("com.unity.modules.nvidia");
                PipelineDefines.AddDefine("DLSS_INSTALLED");
                AssetDatabase.Refresh();
            }
#endif
        }

        protected virtual void DrawHDRPSourceEditWarning()
        {
            EditorGUILayout.HelpBox("HDRP Upscaling requires source file edits. Please read the 'Quick Start: HDRP' chapter in the documentation.", MessageType.Warning);
            if (GUILayout.Button("I have edited the source files!"))
            {
                PipelineDefines.AddDefine("TND_HDRP_EDITEDSOURCE");
                AssetDatabase.Refresh();
            }
        }

        protected virtual void DrawPlatformNotSupportedWarning()
        {
            EditorGUILayout.HelpBox("DLSS is not supported on this platform.", MessageType.Error);
        }

        protected virtual void DrawDLSSSettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("DLSS Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(dlssQualityProp, Styles.qualityText);
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawImageQualitySettings()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Image Quality", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            
            EditorGUILayout.PropertyField(antiGhostingProp, Styles.antiGhostingText);
            EditorGUILayout.PropertyField(sharpeningProp, Styles.sharpeningText);
            
            if (sharpeningProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(sharpnessProp, Styles.sharpnessText);
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawPipelineSpecificNotes()
        {
#if UNITY_URP
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("URP: Ensure 'DLSS Scriptable Render Pass' is added to the Renderer Features. For camera stacking, set 'm_cameraStacking' to true on the base camera.", MessageType.Info);
#endif
#if UNITY_HDRP
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("HDRP: Make sure the upscaler is added to 'Before Post Process' in HDRP Global Settings. See documentation for details.", MessageType.Info);
#endif
        }

        protected static class Styles
        {
            public static GUIContent qualityText = new GUIContent("Quality", "Quality 1.5, Balanced 1.7, Performance 2, Ultra Performance 3");
            public static GUIContent antiGhostingText = new GUIContent("Anti-Ghosting", "The Anti-Ghosting value between 0 and 1, where 0 is no additional anti-ghosting and 1 is maximum additional Anti-Ghosting.");
            public static GUIContent sharpeningText = new GUIContent("Sharpening", "Enable an additional sharpening in the dlss algorithm.");
            public static GUIContent sharpnessText = new GUIContent("Sharpness", "The sharpness value between 0 and 1, where 0 is no additional sharpness and 1 is maximum additional sharpness.");
        }
    }
} 