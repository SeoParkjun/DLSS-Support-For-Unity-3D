Stress Test System for Unity by Inbora Studio
===========================

1. Add the StressTestManager script to an empty GameObject in your scene.
   - Optionally assign a custom cube prefab, or leave blank to use the default.
   - Adjust cubesPerFrame, maxCubes, and spawnArea as needed.

2. Create a Canvas in your scene and add four Text UI elements:
   - Assign them to the fpsText, resolutionText, gpuText, and cpuText fields of the StressTestStatsUI script.
   - Place the StressTestStatsUI script on any GameObject (e.g., the Canvas).

3. Play the scene to start the stress test and see the stats overlay.

4. The cubes will spawn with random positions, rotations, and colors, and will fall with physics.

5. The stats overlay will show FPS, screen resolution, GPU, and CPU info in different colors for clarity and beauty.

Note: For real-time GPU/CPU usage %, you may need a native plugin or external tool, as Unity's C# API does not provide this directly. 