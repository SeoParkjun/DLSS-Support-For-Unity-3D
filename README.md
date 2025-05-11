# DLSS Upscaling for Unity
> ❤️ **Info:** Tested on Unity 6 HDRP 17.0.+

## DLSS HDRP OFF 
![image](https://github.com/user-attachments/assets/635d4a4c-0296-4db8-8563-56c3008c621e)

---
## DLSS HDRP ON
![image](https://github.com/user-attachments/assets/09608033-affb-4f06-a8a4-aa63973365ee)



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
| High-Definition (HDRP) | ⚠️ Native support exists. [Custom override required](HDRP_SUPPORT.md). |

## DLSS UNITY 3D V2.2.6 (Menu)
| Setting               | Scaling Factor | Description                                 |
| --------------------- | -------------- | ------------------------------------------- |
| **Off**               | –              | DLSS is disabled (native resolution only)   |
| **Native AA**         | 1.0×           | Native resolution with anti-aliasing only   |
| **Ultra Quality**     | 1.2×           | Slight upscale for best image quality       |
| **Quality**           | 1.5×           | Balanced between performance and clarity    |
| **Balanced**          | 1.7×           | Optimized for performance with fair quality |
| **Performance**       | 2.0×           | Prioritizes FPS, lower render resolution    |
| **Ultra Performance** | 3.0×           | Maximum FPS boost, lowest render resolution |


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
##  Features (BIRP Only)

- **FallBack Anti-Aliasing**: Automatically switches to fallback AA if DLSS is unsupported.
- **Anti-Ghosting Slider (0-1)**: Adjusts ghosting artifacts from temporal upscaling.
- **Auto Texture Update**: Automatically adjusts MipMap bias for all loaded textures.
- **Mip Map Update Frequency**: Control how often textures are updated.
- **Mipmap Bias Override**: Manually fine-tune the bias to reduce texture flickering.

---
##  Public API

Available via `DLSS_URP.cs` and `DLSS_HDRP.cs` camera components:

```csharp
// Set DLSS quality level
public bool OnSetQuality(DLSS_Quality value);

// Check if DLSS is supported
public bool OnIsSupported();
```
 NVIDIA DLSS for Unity

This package provides NVIDIA DLSS (Deep Learning Super Sampling) integration for Unity's Universal Render Pipeline (URP) and High Definition Render Pipeline (HDRP).

## Features

- DLSS 2.x support for both URP and HDRP
- Quality modes: Quality, Balanced, Performance, and Ultra Performance
- Anti-ghosting and sharpening controls
- Camera stacking support for URP
- Easy setup and configuration

## Requirements

- Unity 2020.3 or newer
- NVIDIA GPU with DLSS support (RTX series or GTX 16 series)
- Windows 10 64-bit
- NVIDIA Game Ready Driver 445.75 or newer

## Installation

1. Import the package into your Unity project
2. Install the NVIDIA DLSS package through Unity Package Manager
3. Follow the pipeline-specific setup instructions below

## URP Setup

1. Add the DLSS component to your main camera
2. Add the DLSS Scriptable Render Pass to your URP Renderer
3. Configure DLSS settings in the inspector
4. For camera stacking, enable the option on the base camera

## HDRP Setup

1. Add the DLSS component to your main camera
2. Add the DLSS upscaler to the HDRP Global Settings
3. Configure DLSS settings in the inspector

## Usage

### Basic Configuration

1. Select your desired DLSS Quality mode:
   - Quality (1.5x upscaling)
   - Balanced (1.7x upscaling)
   - Performance (2.0x upscaling)
   - Ultra Performance (3.0x upscaling)

2. Adjust image quality settings:
   - Anti-Ghosting: Reduces ghosting artifacts (0-1)
   - Sharpening: Enable/disable additional sharpening
   - Sharpness: Control sharpening strength (0-1)

### Advanced Features

#### Camera Stacking (URP only)
Enable camera stacking on the base camera to support multiple cameras with DLSS.

#### Dynamic Resolution
DLSS automatically handles dynamic resolution scaling based on the selected quality mode.

## Troubleshooting

### Common Issues

1. **DLSS not working**
   - Ensure you have an NVIDIA GPU with DLSS support
   - Check that you're using a supported Unity version
   - Verify NVIDIA Game Ready Driver is up to date

2. **Visual artifacts**
   - Adjust anti-ghosting value
   - Try different quality modes
   - Check motion vector setup

3. **Performance issues**
   - Try a lower quality mode
   - Disable sharpening
   - Check GPU driver version

## Support

For issues and feature requests, please visit our GitHub repository or contact support.

## License

This package is licensed under the MIT License. See LICENSE file for details. 
