using UnityEngine;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;

#if UNITY_STANDALONE_WIN && UNITY_64
using UnityEngine.NVIDIA;
#endif

#if DLSS_INSTALLED && UNITY_STANDALONE_WIN && UNITY_64
using NVIDIA = UnityEngine.NVIDIA;
#endif

namespace TND.DLSS
{
    /// <summary>
    /// Base class for DLSS implementation in Unity.
    /// Provides core functionality for both URP and HDRP pipelines.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class DLSS_UTILS : MonoBehaviour
    {
        #region Public Properties
        [Header("DLSS Settings")]
        [Tooltip("Quality mode for DLSS upscaling")]
        public DLSS_Quality DLSSQuality = DLSS_Quality.Balanced;

        [Header("Image Quality")]
        [Range(0f, 1f)]
        [Tooltip("Controls the strength of anti-ghosting effect")]
        public float m_antiGhosting = 0.0f;

        [Tooltip("Enable additional sharpening in the DLSS algorithm")]
        public bool sharpening = false;

        [Range(0f, 1f)]
        [Tooltip("Controls the strength of sharpening effect")]
        public float sharpness = 0.0f;
        #endregion

        #region Protected Fields
        protected Camera m_mainCamera;
        protected bool m_dlssInitialized = false;
        protected float m_scaleFactor = 1.0f;
        protected float m_previousScaleFactor = -1.0f;
        protected int m_displayWidth = 0;
        protected int m_displayHeight = 0;
        protected int m_renderWidth = 0;
        protected int m_renderHeight = 0;
        protected RenderingPath m_previousRenderingPath;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            m_mainCamera = GetComponent<Camera>();
            if (m_mainCamera == null)
            {
                Debug.LogError("DLSS requires a Camera component!");
                enabled = false;
                return;
            }
        }

        protected virtual void OnEnable()
        {
            if (!m_dlssInitialized)
            {
                InitializeDLSS();
            }
        }

        protected virtual void OnDisable()
        {
            if (m_dlssInitialized)
            {
                DisableDLSS();
            }
        }
        #endregion

        #region DLSS Methods
        /// <summary>
        /// Initialize DLSS with the current settings
        /// </summary>
        protected virtual void InitializeDLSS()
        {
            if (!m_dlssInitialized)
            {
                m_displayWidth = m_mainCamera.pixelWidth;
                m_displayHeight = m_mainCamera.pixelHeight;
                m_previousRenderingPath = m_mainCamera.actualRenderingPath;
                m_dlssInitialized = true;
            }
        }

        /// <summary>
        /// Disable DLSS and cleanup resources
        /// </summary>
        protected virtual void DisableDLSS()
        {
            m_dlssInitialized = false;
        }

        /// <summary>
        /// Get the upscale ratio based on DLSS quality mode
        /// </summary>
        protected float GetUpscaleRatioFromQualityMode(DLSS_Quality quality)
        {
            switch (quality)
            {
                case DLSS_Quality.Quality:
                    return 1.5f;
                case DLSS_Quality.Balanced:
                    return 1.7f;
                case DLSS_Quality.Performance:
                    return 2.0f;
                case DLSS_Quality.UltraPerformance:
                    return 3.0f;
                default:
                    return 1.0f;
            }
        }
        #endregion
    }
} 