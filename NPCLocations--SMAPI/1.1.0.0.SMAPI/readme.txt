Adaptation of Kemenor's Storm mod, NPCLocations, to SMAPI.  Shows a menu listing current NPC locations when you press Z or LeftShoulder on your controller.
Kemenor's code, adapted for SMAPI by cantorsdust

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSTALLATION
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

The zip contains one folder, NPCLocations, with three files NPCLocations.dll, config.json, and manifest.json. The folder must be placed in %appdata%\StardewValley\Mods.

Thus, the total path for all 3 of the files required for this mod to function are:
%appdata%\StardewValley\Mods\NPCLocations\NPCLocations.dll

AND

%appdata%\StardewValley\Mods\NPCLocations\config.json


AND

%appdata%\StardewValley\Mods\NPCLocations\manifest.json


REQUIRES SMAPI 0.38+ to be installed!  Version 1.0.0.0.SMAPI is for SMAPI 0.37
https://github.com/ClxS/SMAPI/releases

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
USAGE
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Run StardewModdingAPI.exe in your main Stardew Valley folder. This will load the mods and then start the game.

Please note that this game comes with a config.json file with nine options:

"ButtonKey": "LeftShoulder",
"KeyboardKey": "Z


You may configure these keys by editing the values shown.  
Keyboard keys should be capitalized.  
Possible ButtonKey values are listed here in the Member Name column: https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.buttons.aspx


-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
CHANGELOG
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
1.1.0.0.SMAPI
Updated mod for SMAPI 0.38, included mod manifest.

1.0.0.0.SMAPI
First SMAPI port