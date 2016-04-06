/*
    SMAPI version by cantorsdust
 */

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCLocations
{
    public class NPCLocations : Mod
    {
        public static ModConfig NPCLocationsConfig { get; private set; }

        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("NPCLocations Has Loaded");
            ControlEvents.KeyPressed += Events_KeyPressed;
            ControlEvents.ControllerButtonPressed += Events_ControllerPressed;


        }
        public bool showNPC = false;

        void runConfig()
        {
            NPCLocationsConfig = new ModConfig().InitializeConfig(BaseConfigPath);
        }


        public void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            //Console.WriteLine("Key pressed: " + e.KeyPressed.ToString());
            if (e.KeyPressed.ToString().Equals(NPCLocationsConfig.KeyboardKey))
            {
                HandleMenu();
            }
        }

        public void Events_ControllerPressed(object sender, EventArgsControllerButtonPressed e)
        {
            //Console.WriteLine("Controller button pressed: " + e.ButtonPressed.ToString());
            if (e.ButtonPressed.ToString().Equals(NPCLocationsConfig.ButtonKey))
            {
                HandleMenu();
            }
        }

        public void HandleMenu()
        {
            if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
            {
                Game1.activeClickableMenu = new NPCMenu();
                NPCMenu menu = (NPCMenu)Game1.activeClickableMenu;
                menu.initializeUpperRightCloseButton();
                menu.invisible = false;
                showNPC = true;
            }
            else if (showNPC)
            {
                Game1.activeClickableMenu = null;
                showNPC = false;
            }
        }
    }

    public class ModConfig : Config
    {
        public string ButtonKey { get; set; }
        public string KeyboardKey { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            ButtonKey = "LeftShoulder";
            KeyboardKey = "Z";
            return this as T;
        }
    }
}
