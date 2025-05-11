# HDRP Support Integration Guide

This README provides detailed steps for integrating **DLSS** and enabling a custom upscaler (`TND`) within Unity's **High Definition Render Pipeline (HDRP)**.  
It supports Unity versions **2022.3 LTS**, **2023.1**, and **Unity 6+**, with HDRP versions ranging from **13.x.x** to **17.x.x**.

> ⚠️ This guide assumes you are using Unity with HDRP already set up in your project.

---

## 📦 Quick Start: HDRP Integration Steps

### ✅ Step 1: Import DLSS Package
- Import **DLSS Unity 3D V2.2.6** into your project.

---

## 🛠️ Step 2: Make HDRP Source Editable

### Editing Render Pipeline Source Files

1. In Unity Editor, locate the **Packages** folder.
2. Right-click `High Definition RP` → Select `Show in Explorer`.
3. Copy the folder:  
   `com.unity.render-pipelines.high-definition@xx.x.xx`
4. Paste it into your local `Packages` folder (same directory as `Assets/`, `ProjectSettings/`, etc).

> HDRP source files are now editable from within your project.

---

## ✏️ Step 3: Modify HDRP Source Files

### 🔧 File 1: `HDCamera.cs`

Path:  
`Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Camera/HDCamera.cs`

---

### 🧩 For Unity 2022.3 LTS or older (HDRP 13.x.x)

Replace:
```csharp
internal bool UpsampleHappensBeforePost()
{
    return IsDLSSEnabled() || IsTAAUEnabled();
}
```

With:
```csharp
public bool tndUpscalerEnabled = false;

internal bool IsTNDUpscalerEnabled()
{
    return tndUpscalerEnabled;
}

internal bool UpsampleHappensBeforePost()
{
    return IsTNDUpscalerEnabled() || IsDLSSEnabled() || IsTAAUEnabled();
}
```

---

### 🧩 For Unity 2023.1 or HDRP 14.0.x

Replace:
```csharp
internal DynamicResolutionHandler.UpsamplerScheduleType UpsampleSyncPoint()
{
    if (IsDLSSEnabled())
        return HDRenderPipeline.currentAsset.currentPlatformRenderPipelineSettings.dynamicResolutionSettings.DLSSInjectionPoint;
    else if (IsTAAUEnabled())
        return DynamicResolutionHandler.UpsamplerScheduleType.BeforePost;
    else
        return DynamicResolutionHandler.UpsamplerScheduleType.AfterPost;
}
```

With:
```csharp
public bool tndUpscalerEnabled = false;

internal bool IsTNDUpscalerEnabled()
{
    return tndUpscalerEnabled;
}

internal DynamicResolutionHandler.UpsamplerScheduleType UpsampleSyncPoint()
{
    if (IsDLSSEnabled())
        return HDRenderPipeline.currentAsset.currentPlatformRenderPipelineSettings.dynamicResolutionSettings.DLSSInjectionPoint;
    else if (IsTAAUEnabled())
        return DynamicResolutionHandler.UpsamplerScheduleType.BeforePost;
    else if (IsTNDUpscalerEnabled())
        return DynamicResolutionHandler.UpsamplerScheduleType.BeforePost;
    else
        return DynamicResolutionHandler.UpsamplerScheduleType.AfterPost;
}
```

---

### 🧩 For Unity 6+ (HDRP 17.0.x)

Locate the function:
```csharp
internal DynamicResolutionHandler.UpsamplerScheduleType UpsampleSyncPoint()
```

- Add this **above the function**:
```csharp
public bool tndUpscalerEnabled = false;

internal bool IsTNDUpscalerEnabled()
{
    return tndUpscalerEnabled;
}
```

- Then in the method body, **modify**:
```csharp
if (IsFSR2Enabled() || IsTNDUpscalerEnabled())
{
    return HDRenderPipeline.currentAsset.currentPlatformRenderPipelineSettings.dynamicResolutionSettings.FSR2InjectionPoint;
}
```

---

### 🔧 File 2: `HDRenderPipeline.PostProcess.cs`

Path:  
`Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/HDRenderPipeline.PostProcess.cs`

#### For versions before Unity 6 (HDRP 15.x.x or older)

Find:
```csharp
source = CustomPostProcessPass(renderGraph, hdCamera, source, depthBuffer, normalBuffer, motionVectors, m_GlobalSettings.beforePostProcessCustomPostProcesses, HDProfileId.CustomPostProcessBeforePP);
```

Replace with:
```csharp
source = CustomPostProcessPass(renderGraph, hdCamera, source, depthBuffer, normalBuffer, motionVectors, m_GlobalSettings.beforePostProcessCustomPostProcesses, HDProfileId.CustomPostProcessBeforePP);             
if (hdCamera.IsTNDUpscalerEnabled())
{
    SetCurrentResolutionGroup(renderGraph, hdCamera, ResolutionGroup.AfterDynamicResUpscale);
}
```

#### For Unity 6+ (HDRP 17.0.x)

Find:
```csharp
source = BeforeCustomPostProcessPass(renderGraph, hdCamera, source, depthBuffer, normalBuffer, motionVectors, m_CustomPostProcessOrdersSettings.beforePostProcessCustomPostProcesses, HDProfileId.CustomPostProcessBeforePP);
```

Replace with:
```csharp
source = BeforeCustomPostProcessPass(renderGraph, hdCamera, source, depthBuffer, normalBuffer, motionVectors, m_CustomPostProcessOrdersSettings.beforePostProcessCustomPostProcesses, HDProfileId.CustomPostProcessBeforePP);             
if (hdCamera.IsTNDUpscalerEnabled())
{
    SetCurrentResolutionGroup(renderGraph, hdCamera, ResolutionGroup.AfterDynamicResUpscale);
}
```

---

## 🧷 Step 4: Add `TND_DLSS_DLSSRenderPass`

- Add the pass to `HDRenderPipelineGlobalSettings` in “Before Post Process”.

---

## ⚙️ Step 5: HDRP Settings Configuration

- Enable **Dynamic Resolution** in HDRP settings.
- Set **Minimum Screen Percentage** to `33`.
- Set **Default Upscale Filter** to `Catmull Rom`.

> ⚠️ If using multiple Render Pipeline Assets (for different quality levels), configure each of them.

---

## 🎥 Step 6: Camera Configuration

- Add `DLSS_HDRP.cs` script to your **Main Camera**.
- Press the button: **“I have edited the source files”**.

> 🔍 If you get compile errors, the source file edits were incomplete.

- Make sure your camera's **Volume Mask** includes the camera's layer.
- Enable **Allow Dynamic Resolution** on the camera.

> 💡 DLSS will automatically adjust these if skipped, and disable Unity's native DLSS if needed.

---

✅ You're all set to run DLSS with HDRP and TND support in Unity!
