#ifndef DLSS_COMMON_INCLUDED
#define DLSS_COMMON_INCLUDED

// DLSS Quality Mode Constants
#define DLSS_QUALITY_MODE_QUALITY 1.5
#define DLSS_QUALITY_MODE_BALANCED 1.7
#define DLSS_QUALITY_MODE_PERFORMANCE 2.0
#define DLSS_QUALITY_MODE_ULTRA_PERFORMANCE 3.0

// DLSS Buffer Names
#define DLSS_OUTPUT_BUFFER_NAME "DLSS Output"
#define DLSS_COLOR_BUFFER_NAME "DLSS Color"
#define DLSS_DEPTH_BUFFER_NAME "DLSS Depth"
#define DLSS_MOTION_VECTOR_BUFFER_NAME "DLSS Motion Vectors"

// DLSS Common Functions
float GetDLSSUpscaleRatio(float qualityMode)
{
    switch (qualityMode)
    {
        case 0: return DLSS_QUALITY_MODE_QUALITY;
        case 1: return DLSS_QUALITY_MODE_BALANCED;
        case 2: return DLSS_QUALITY_MODE_PERFORMANCE;
        case 3: return DLSS_QUALITY_MODE_ULTRA_PERFORMANCE;
        default: return 1.0;
    }
}

// DLSS Jitter Functions
float2 GetDLSSJitterOffset(float2 uv, float2 resolution, float frameCount)
{
    // Halton sequence for jittering
    float2 jitter;
    jitter.x = frac(frameCount * 0.75487766624669276);
    jitter.y = frac(frameCount * 0.569840290998);
    return (jitter - 0.5) / resolution;
}

// DLSS Motion Vector Functions
float2 GetMotionVector(float2 uv, float2 previousUV)
{
    return uv - previousUV;
}

// DLSS Sharpening Functions
float3 ApplyDLSSSharpening(float3 color, float sharpness)
{
    // Simple sharpening kernel
    float3 sharpened = color * (1.0 + sharpness);
    return lerp(color, sharpened, sharpness);
}

// DLSS Anti-Ghosting Functions
float3 ApplyDLSSAntiGhosting(float3 currentColor, float3 previousColor, float antiGhostingStrength)
{
    return lerp(currentColor, previousColor, antiGhostingStrength);
}

#endif // DLSS_COMMON_INCLUDED 