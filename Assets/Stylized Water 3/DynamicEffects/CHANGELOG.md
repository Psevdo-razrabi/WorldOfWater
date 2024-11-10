3.0.1

• Updated to reflect changes in Stylized Water 3 v3.0.1

Added:
- Generic height modifier prefab effect

Changed:
- Shoreline Wave Spawner, added the option to hide the spawned objects

Fixed:
- Effect not updating while manipulating an object when Scale By Transform was enabled
- Scale By Transform option not respective negative scale values

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