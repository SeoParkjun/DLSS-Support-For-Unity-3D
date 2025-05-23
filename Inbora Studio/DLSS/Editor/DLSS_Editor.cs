using UnityEngine;
using UnityEditor;

#if UNITY_STANDALONE_WIN && UNITY_64
using UnityEngine.NVIDIA;

#endif
#if DLSS_INSTALLED
using NVIDIA = UnityEngine.NVIDIA;
#endif

#if !UPSCALING_HDRP_EDITEDSOURCE
using System.IO;
#endif
namespace TND.DLSS
{
    [CustomEditor(typeof(DLSS_UTILS), editorForChildClasses: true)]
    public class DLSS_Editor : Editor
    {
        public override void OnInspectorGUI() {
#if !DLSS_INSTALLED
#if UNITY_URP || UNITY_HDRP
            EditorGUILayout.HelpBox("Missing NVIDIA DLSS Package. Please install to enable DLSS features.", MessageType.Error);
            if(GUILayout.Button("Install Package")) {
                UnityEditor.PackageManager.Client.Add("com.unity.modules.nvidia");
                PipelineDefines.AddDefine("DLSS_INSTALLED");
                AssetDatabase.Refresh();
            }
#endif
#elif !TND_HDRP_EDITEDSOURCE && UNITY_HDRP
            EditorGUILayout.HelpBox("HDRP Upscaling requires source file edits. Please read the 'Quick Start: HDRP' chapter in the documentation.", MessageType.Warning);
            if (GUILayout.Button("I have edited the source files!"))
            {
                PipelineDefines.AddDefine("TND_HDRP_EDITEDSOURCE");
                AssetDatabase.Refresh();
            }
#elif TND_DLSS

#if !UNITY_STANDALONE_WIN || !UNITY_64
            EditorGUILayout.HelpBox("DLSS is not supported on this platform.", MessageType.Error);
#endif
            DLSS_UTILS dlssScript = target as DLSS_UTILS;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("DLSS Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            DLSS_Quality dlssQuality = (DLSS_Quality)EditorGUILayout.EnumPopup(Styles.qualityText, dlssScript.DLSSQuality);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Image Quality", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            float antiGhosting = dlssScript.m_antiGhosting;
            antiGhosting = EditorGUILayout.Slider(Styles.antiGhostingText, dlssScript.m_antiGhosting, 0.0f, 1.0f);

            bool sharpening = EditorGUILayout.Toggle(Styles.sharpeningText, dlssScript.sharpening);
            float sharpness = dlssScript.sharpness;
            if (dlssScript.sharpening)
            {
                EditorGUI.indentLevel++;
                sharpness = EditorGUILayout.Slider(Styles.sharpnessText, dlssScript.sharpness, 0.0f, 1.0f);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            // Pipeline-specific notes
#if UNITY_URP
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("URP: Ensure 'DLSS Scriptable Render Pass' is added to the Renderer Features. For camera stacking, set 'm_cameraStacking' to true on the base camera.", MessageType.Info);
#endif
#if UNITY_HDRP
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("HDRP: Make sure the upscaler is added to 'Before Post Process' in HDRP Global Settings. See documentation for details.", MessageType.Info);
#endif

            EditorGUILayout.Space();

            if(EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(dlssScript);
                Undo.RecordObject(target, "Changed DLSS Settings");
                dlssScript.DLSSQuality = dlssQuality;
                dlssScript.m_antiGhosting = antiGhosting;
                dlssScript.sharpening = sharpening;
                dlssScript.sharpness = sharpness;
            }
#endif
        }

        private static class Styles
        {
            public static GUIContent qualityText = new GUIContent("Quality", "Quality 1.5, Balanced 1.7, Performance 2, Ultra Performance 3");
            public static GUIContent antiGhostingText = new GUIContent("Anti-Ghosting", "The Anti-Ghosting value between 0 and 1, where 0 is no additional anti-ghosting and 1 is maximum additional Anti-Ghosting.");
            public static GUIContent sharpeningText = new GUIContent("Sharpening", "Enable an additional sharpening in the dlss algorithm.");
            public static GUIContent sharpnessText = new GUIContent("Sharpness", "The sharpness value between 0 and 1, where 0 is no additional sharpness and 1 is maximum additional sharpness.");

        }
    }
}
