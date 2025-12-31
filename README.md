# CommunityPatches
Community patches for RTB

> [!CAUTION]
> These patches are developed by the community to enhance RaceTrackBuilder. Although we’ve tested them as thoroughly as possible, they may not always work as expected and could potentially break your project. Always keep backups before using them. Use at your own risk — we can’t be held responsible for any lost data.


---

## Note:

RaceTrackBuilder is obfuscated, meaning class and member names may change with each update. The patches currently rely on hardcoded names, so they typically need to be updated for most RTB releases. An alternative approach is to target unique class and member signatures, but this makes locating and debugging issues significantly more difficult when those signatures change. We are actively working on a more robust solution.


## Current patches:

<details>
  <summary>Direct FBX/Collada import by dragging and dropping a file into the application (No XPacker)</summary>
  
  Every project contains an xpack called "MyPack", which is just a folder instead of the usual zip file. The patch calls existing methods to import fbx/collada files at runtime into this specific "xpack" and making it instantly available for use.
  These imports use the name of the fbx file as name for the object. The object can be re-imported as many times as you wish - already placed objects will visually update if you load the project again. **Please never delete files manually.**
  
</details>