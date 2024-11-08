3.0.0

What’s new with Stylized Water 3?

• Rewritten rendering code for Render Graph
• Effects are now fully compatible with the SRP Batcher and new GPU Resident Drawer, this minimizes drawcalls
• New shoreline wave spawner component, spawns the shoreline wave prefab along a spline + snaps an audio emitter to the spline, following the camera.
• The render feature is now embedded into the core SW3 render feature
• Effects are now readable through C#

Changed:
- Dynamic Effects component now works based off a "template" material
- Effect displacement can now scale with the Transform
- Normal strength scalable by height