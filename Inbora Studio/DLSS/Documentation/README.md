# NVIDIA DLSS for Unity

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