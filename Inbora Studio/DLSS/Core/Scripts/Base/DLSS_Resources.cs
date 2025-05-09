using UnityEngine;
using System.Collections.Generic;

namespace TND.DLSS
{
    /// <summary>
    /// ScriptableObject for storing DLSS resources and default settings.
    /// This allows for centralized configuration and easy updates.
    /// </summary>
    [CreateAssetMenu(fileName = "DLSS_Resources", menuName = "DLSS/Resources")]
    public class DLSS_Resources : ScriptableObject
    {
        [Header("Default Settings")]
        [Tooltip("Default DLSS quality mode (0: Quality, 1: Balanced, 2: Performance, 3: Ultra Performance)")]
        public int defaultQualityMode = 1;

        [Range(0f, 1f)]
        [Tooltip("Default anti-ghosting strength")]
        public float defaultAntiGhosting = 0f;

        [Tooltip("Default sharpening state")]
        public bool defaultSharpening = false;

        [Range(0f, 1f)]
        [Tooltip("Default sharpness value")]
        public float defaultSharpness = 0.5f;

        [Header("Resolution Limits")]
        [Tooltip("Minimum supported resolution width")]
        public int minResolutionWidth = 1280;

        [Tooltip("Minimum supported resolution height")]
        public int minResolutionHeight = 720;

        [Tooltip("Maximum supported resolution width")]
        public int maxResolutionWidth = 7680;

        [Tooltip("Maximum supported resolution height")]
        public int maxResolutionHeight = 4320;

        [Header("System Requirements")]
        [Tooltip("List of supported platforms")]
        public List<string> supportedPlatforms = new List<string> { "Windows" };

        [Tooltip("List of supported NVIDIA GPUs")]
        public List<string> supportedGPUs = new List<string>
        {
            "RTX 2060",
            "RTX 2070",
            "RTX 2080",
            "RTX 3060",
            "RTX 3070",
            "RTX 3080",
            "RTX 3090",
            "GTX 1660",
            "GTX 1660 Ti"
        };

        [Tooltip("Minimum required NVIDIA driver version")]
        public string minDriverVersion = "445.75";

        private static DLSS_Resources instance;
        public static DLSS_Resources Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<DLSS_Resources>("DLSS_Resources");
                    if (instance == null)
                    {
                        Debug.LogError("DLSS_Resources asset not found in Resources folder!");
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Check if the current resolution is supported
        /// </summary>
        public bool IsResolutionSupported(int width, int height)
        {
            return width >= minResolutionWidth && width <= maxResolutionWidth &&
                   height >= minResolutionHeight && height <= maxResolutionHeight;
        }

        /// <summary>
        /// Get the default DLSS quality mode
        /// </summary>
        public DLSS_Quality GetDefaultQualityMode()
        {
            return (DLSS_Quality)defaultQualityMode;
        }

        /// <summary>
        /// Check if the current platform is supported
        /// </summary>
        public bool IsPlatformSupported()
        {
            return supportedPlatforms.Contains(Application.platform.ToString());
        }
    }
} 