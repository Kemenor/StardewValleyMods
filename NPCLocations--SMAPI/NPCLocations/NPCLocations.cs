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
        public override string Name
        {
            get { return "NPCLocations"; }
        }

        public override string Authour
        {
            get { return "Kemenor's code, adapted for SMAPI by cantorsdust"; }
        }

        public override string Version
        {
            get { return "1.0.0.0.SMAPI"; }
        }

        public override string Description
        {
            get { return "Adaptation of Kemenor's Storm mod to SMAPI.  Shows a menu listing current NPC locations when you press Z"; }
        }


        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("NPCLocations Has Loaded");
            ControlEvents.KeyPressed += Events_KeyPressed;
            ControlEvents.ControllerButtonPressed += Events_ControllerPressed;


        }

        public string ButtonKey = "LeftShoulder";
        public string KeyboardKey = "Z";
        public bool showNPC = false;

        void runConfig()
        {
            
            string ConfigPathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\NPCLocations.ini");
            string ConfigPathSVMods = "Mods\\NPCLocations.ini";
            string DLLPathAppData = Environment.ExpandEnvironmentVariables("%AppData%\\StardewValley\\Mods\\NPCLocations.dll");
            string DLLPathSVMods = "Mods\\NPCLocations.dll";
            string path = null;
            char[] delimiterChars = { '=' };
            if (File.Exists(ConfigPathAppData))
            {
                Console.WriteLine("found INI in %appdata%");
                path = ConfigPathAppData;
            }
            else if (File.Exists(ConfigPathSVMods))
            {
                Console.WriteLine("found INI in Stardew Valley-Mods");
                path = ConfigPathSVMods;
            }
            else
            {
                Console.WriteLine("WARNING:  Could not find INI.  Writing new INI with default values next to DLL");
                if (File.Exists(DLLPathAppData))
                {
                    File.AppendAllLines(ConfigPathAppData, new[] { "ButtonKey=LeftShoulder", "KeyboardKey=Z" });

                }
                else if (File.Exists(DLLPathSVMods))
                {
                    File.AppendAllLines(ConfigPathAppData, new[] { "ButtonKey=LeftShoulder", "KeyboardKey=Z" });
                }

            }

            if (path != null)
            {
                List<string> fileData = File.ReadAllLines(path).ToList();
                //each fileData[index] is a line
                //we'll check to see what each line holds and parse that line's data
                foreach (string line in fileData)
                {
                    //Console.WriteLine("Parsing INI line " + index.ToString("g"));
                    string[] words = line.Split(delimiterChars);
                    //make sure you've fixed old INIs
                    if (words[0].Contains("ButtonKey"))
                    {
                        ButtonKey = words[1];
                        Console.WriteLine("ButtonKey is " + ButtonKey);
                    }
                    else if (words[0].Contains("KeyboardKey"))
                    {

                        KeyboardKey = words[1];
                        Console.WriteLine("KeyboardKey is " + KeyboardKey);
                    }
                }
            }            
        }


        public void Events_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            //Console.WriteLine("Key pressed: " + e.KeyPressed.ToString());
            if (e.KeyPressed.ToString().Equals(KeyboardKey))
            {
                HandleMenu();
            }
        }

        public void Events_ControllerPressed(object sender, EventArgsControllerButtonPressed e)
        {
            //Console.WriteLine("Controller button pressed: " + e.ButtonPressed.ToString());
            if (e.ButtonPressed.ToString().Equals(ButtonKey))
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
}
