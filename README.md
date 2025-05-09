# DLSS Upscaling for Unity

## DLSS HDRP OFF -- Tested on 2022+ & Unity 6
![Screenshot 2025-05-07 165235](https://github.com/user-attachments/assets/1dbe1692-63bf-4f62-9281-d63a37c6cf49)
---
## DLSS HDRP ON
![Screenshot 2025-05-07 165415](https://github.com/user-attachments/assets/e82e218c-66fc-4a65-bc70-2e076a0eacdb)


**by [Inbora Studio](https://github.com/inborastudio)**  
NVIDIA DLSS integration for Unity's Built-in (BIRP), Universal (URP), and High-Definition Render Pipeline (HDRP).

##  About DLSS

**Deep Learning Super Sampling (DLSS)** is a performance-enhancing upscaling technique developed by NVIDIA. It renders frames at a lower resolution and then uses AI to upscale them to a higher resolution, offering nearly-native visual quality with improved performance.

> **Note**: DLSS only works on NVIDIA RTX GPUs.

### Benefits

- Increased framerate on GPU-bound projects
- Lower GPU usage and improved battery life on laptops
- High-quality anti-aliasing at lower costs

---

##  Supported Render Pipelines

| Render Pipeline | Support Status |
|----------------|----------------|
| Built-in (BIRP) | ✅ |
| Universal (URP) | ✅ |
| High-Definition (HDRP) | ⚠️ Native support exists. Custom override required. |

##  Supported Platforms

- Windows x64 (DX11, DX12)
- PCVR (BIRP & URP)

>  DLSS support for HDRP in PCVR is **currently in development**.

---

##  Quick Start

### URP Setup

1. Import the **DLSS Package** into your Unity project.
2. Add `DLSS_URP.cs` to your **Main Camera**.
3. Add **DLSS Scriptable Render Feature** to each `UniversalRendererData` used in your project.
4. Hit **Play**!

> If the `DLSS_URP` component cannot be added to the camera, verify that you have a valid Scriptable Render Pipeline asset set in your Graphics settings.

---

### HDRP Setup

1. Import the **DLSS Package**.
2. **Edit HDRP source files** (refer to "HDRP Source Files" chapter in documentation).
3. Add `TND_DLSS_DLSSRenderPass` to `HDRenderPipelineGlobalSettings` → **Before Post Process**.
4. Enable **Dynamic Resolution** in HDRP settings:
   - Minimum Screen Percentage: `33`
   - Upscale Filter: `Catmull-Rom`
5. Add `DLSS_HDRP.cs` to the **Main Camera**.
6. Press the **“I have edited the source files”** button.

>  If you see compilation errors, HDRP source files were not properly edited.

#### Optional HDRP Steps

- Ensure the **Volume Mask** of the camera includes the camera's layer.
- Enable **Allow Dynamic Resolution** on the camera.

---

##  Inspector Settings

### Quality Modes

| Setting            | Scale Factor | Description                  |
|--------------------|--------------|------------------------------|
| Off                | -            | DLSS disabled                |
| Native AA          | 1.0x         | Native resolution with AA    |
| Ultra Quality      | 1.2x         | High visual fidelity         |
| Quality            | 1.5x         | Balanced quality & speed     |
| Balanced           | 1.7x         | Lower render resolution      |
| Performance        | 2.0x         | High performance             |
| Ultra Performance  | 3.0x         | Maximum performance          |

> Example: At 1920x1080 native resolution, "Performance" mode renders at 960x540 and upscales to 1080p.

---

##  Features (BIRP Only)

- **FallBack Anti-Aliasing**: Automatically switches to fallback AA if DLSS is unsupported.
- **Anti-Ghosting Slider (0-1)**: Adjusts ghosting artifacts from temporal upscaling.
- **Auto Texture Update**: Automatically adjusts MipMap bias for all loaded textures.
- **Mip Map Update Frequency**: Control how often textures are updated.
- **Mipmap Bias Override**: Manually fine-tune the bias to reduce texture flickering.

---

##  Unity 6+ (HDRP 17.x) Source Patch
- For Unity 6 or newer using HDRP 17.0.x, you need to manually patch the HDRP source.

##  Step-by-Step
- Locate the following line in your HDRP source file (usually in a render loop file):

```csharp
source = BeforeCustomPostProcessPass(renderGraph, hdCamera, source,
    depthBuffer, normalBuffer, motionVectors,
    m_CustomPostProcessOrdersSettings.beforePostProcessCustomPostProcesses,
    HDProfileId.CustomPostProcessBeforePP);
```
- Replace it with:

```csharp
source = BeforeCustomPostProcessPass(renderGraph, hdCamera, source,
    depthBuffer, normalBuffer, motionVectors,
    m_CustomPostProcessOrdersSettings.beforePostProcessCustomPostProcesses,
    HDProfileId.CustomPostProcessBeforePP);
    
if (hdCamera.IsTNDUpscalerEnabled())
{
    SetCurrentResolutionGroup(renderGraph, hdCamera,
        ResolutionGroup.AfterDynamicResUpscale);
}
```

##  Public API

Available via `DLSS_URP.cs` and `DLSS_HDRP.cs` camera components:

```csharp
// Set DLSS quality level
public bool OnSetQuality(DLSS_Quality value);

// Check if DLSS is supported
public bool OnIsSupported();
