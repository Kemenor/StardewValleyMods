# StardewValleyMods
The Source code of all my Stardew Valley Mods.

These mods require the Storm API to work:  http://community.playstarbound.com/threads/storm-modding-api-literally-use-1-05-before-posting-i-will-know.108484/
I will write what version of the Storm API I used for the releases, it should work with those versions.

#Current Mods
##NPCLocation
Will show a menu with the NPC names and their locations open the menu with the "z" key and close it with "escape".
The open key is now configurable:
```json
{
  "Key": "Z"
}
```

##Usefull Sprinklers
Will make sprinklers more usefull. Sprinkler will now water an area around them, configurable in the config.json.
the number indicates the rectangle area that should be watered.
```json
{
  "Sprinkler": 3,
  "QualitySprinkler": 5,
  "IridiumSprinkler": 7
}
```
with this config the normal sprinkler will water a 3x3 area.
