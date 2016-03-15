using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Accessor;
using Storm.StardewValley.Event;
using Storm.StardewValley.Proxy;
using Storm.StardewValley.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Storm;

namespace NPCLocations
{
	[Mod]
	public class NPCLocations : DiskResource
	{
		private bool showNPC = false;
		private NPCMenu locs;
		private Keys openKey;
		public static NPCLocationConfig NConfig { get; private set; }
		[Subscribe]
		public void init(InitializeEvent @event)
		{
			locs = new NPCMenu(@event.Root);
			NConfig = new NPCLocationConfig();
			NConfig = (NPCLocationConfig)Config.InitializeConfig(Config.GetBasePath(this), NConfig);
			if (Enum.IsDefined(typeof(Keys), NConfig.Key.ToUpper()))
			{
				openKey = (Keys)Enum.Parse(typeof(Keys), NConfig.Key.ToUpper());
			}
			else
			{
				openKey = Keys.Z;
			}

		}

		[Subscribe]
		public void KeyPressedCallback(KeyPressedEvent @event)
		{
			//only react to the z key
			if (@event.Key == openKey)

			{
				if (@event.Root.ActiveClickableMenu != null && showNPC)
				{
					@event.Root.ActiveClickableMenu = null;
					showNPC = false;
				}
				else
				{
					locs.root = @event.Root;
					// create our menu
					//set it as ActiveMenu per Proxy
					@event.Root.ActiveClickableMenu = @event.Proxy<ClickableMenuAccessor, ClickableMenu>(locs);
					//set our draw to true
					showNPC = true;
				}
			}
		}

		[Subscribe]
		public void PostRender(PreUIRenderEvent @event)
		{
			//do we need to draw our "menu"
			if (@event.Root.ActiveClickableMenu != null && showNPC)
			{
				locs.Draw(@event.Root.SpriteBatch);
			}
			//our menu was probably closed so set draw to falses
			if (@event.Root.ActiveClickableMenu == null && showNPC)
			{
				showNPC = false;
			}
		}
		class NPCMenu : ClickableMenuDelegate
		{
			public StaticContext root { get; set; }

			public NPCMenu(StaticContext root)
			{
				this.root = root;
			}
			public override void Draw(SpriteBatch b)
			{
				Texture2D MenuTiles = this.root.Content.Load<Texture2D>("MenuTiles");
				var font = root.SmallFont;
				var viewport = root.Viewport;
				var textColor = Color.Black;

				//how much padding do we want on the sides
				int leftRightPadding = 100;
				int upperLowerPadding = 100;

				//calculate the dimensions of the menu
				int width = viewport.Width - leftRightPadding * 2;
				int height = viewport.Height - upperLowerPadding * 2;

				//Texture2D for the menu
				Texture2D menu = new Texture2D(root.Graphics.GraphicsDevice, width, height);
				//get the upper left corner of the menu
				Vector2 screenLoc = new Vector2(leftRightPadding, upperLowerPadding);
				//where do we start to draw our strings
				Vector2 locationDraw = screenLoc + new Vector2(10, 10);

				//whats the widest String in the current column
				float widest = 0;
				//offset for the columns
				float offset = 0;

				//fill menu with dump data so it shows
				var data = new uint[width * height];
				for (int i = 0; i < data.Length; i++)
				{
					data[i] = 0xffffffff;
				}
				menu.SetData<uint>(data);

				//draw the ugly menu
				Vector2 menubar = screenLoc - new Vector2(32, 32);
				b.Draw(menu, screenLoc, new Color(232, 207, 128));

				Rectangle upperLeft = new Rectangle(0, 0, 64, 64);
				Rectangle upperRight = new Rectangle(192, 0, 64, 64);
				Rectangle lowerLeft = new Rectangle(0, 192, 64, 64);
				Rectangle lowerRight = new Rectangle(192, 192, 64, 64);
				Rectangle upperBar = new Rectangle(128, 0, 64, 64);
				Rectangle leftBar = new Rectangle(0, 128, 64, 64);
				Rectangle rightBar = new Rectangle(192, 128, 64, 64);
				Rectangle lowerBar = new Rectangle(128, 192, 64, 64);

				float menuHeight = height + 2 * 32;
				float menuWidth = width + 2 * 32;

				float rightUpperCorner = menuWidth - 2 * 64;
				float leftLowerCorner = menuHeight - 2 * 64;

				b.Draw(MenuTiles, menubar, upperLeft, Color.White);
				//Draw upperbar
				for (int i = 64; i < rightUpperCorner; i += 64)
				{
					b.Draw(MenuTiles, menubar + new Vector2(i, 0), upperBar, Color.White);
				}
				//draw upper right corner
				b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner - 1, 0), upperRight, Color.White);

				//draw left bar
				for (int i = 64; i < leftLowerCorner; i++)
				{
					b.Draw(MenuTiles, menubar + new Vector2(0, i), leftBar, Color.White);
				}
				//draw lower left corner
				b.Draw(MenuTiles, menubar + new Vector2(0, leftLowerCorner + 64 - 1), lowerLeft, Color.White);

				//draw right bar
				for (int i = 64; i < leftLowerCorner; i++)
				{
					b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner + 64 - 1, i), rightBar, Color.White);
				}
				//draw right Corner
				b.Draw(MenuTiles, menubar + new Vector2(rightUpperCorner + 64 - 1, leftLowerCorner + 64 - 1), lowerRight, Color.White);

				//draw lower Bar
				for (int i = 64; i < rightUpperCorner; i++)
				{
					b.Draw(MenuTiles, menubar + new Vector2(i, leftLowerCorner + 64 - 1), lowerBar, Color.White);
				}


				//put all npc location information in a set
				SortedSet<string> npcLocation = new SortedSet<string>();
				for (int i = 0; i < root.Locations.Count; i++)
				{
					GameLocation location = root.Locations[i];
					for (int j = 0; j < location.Characters.Count; j++)
					{
						npcLocation.Add(location.Characters[j].Name + ": " + location.Name);
					}
				}

				//draw the locations on the "menu"
				foreach (string item in npcLocation)
				{
					//how big is the string
					Vector2 fontHeight = font.MeasureString(item);
					//if the new string is wider than the others set new widest

					//check if the current string has enough space in our "menu"
					if (locationDraw.Y + fontHeight.Y > viewport.Height - upperLowerPadding)
					{
						//start the next column because the current one doesn't have enough space
						locationDraw = screenLoc + new Vector2(10, 10);
						locationDraw.X += widest + offset + 20;
						offset += widest;
						widest = 0;
					}
					b.DrawString(font, item, locationDraw, textColor);
					locationDraw.Y += fontHeight.Y;
					if (fontHeight.X > widest)
					{
						widest = fontHeight.X;
					}

				}
			}

			public override void EmergencyShutDown()
			{
			}

			public override void PerformHoverAction(int x, int y)
			{
			}

			public override bool ReadyToClose()
			{
				return true;
			}

			public override void ReceiveGamePadButton(Buttons b)
			{
			}

			public override void ReceiveKeyPress(Keys key)
			{
			}

			public override void ReceiveLeftClick(int x, int y, bool playSound = true)
			{
			}

			public override void ReceiveRightClick(int x, int y, bool playSound = true)
			{
			}

			public override void ReceiveScrollWheelAction(int direction)
			{
			}

			public override void Update(GameTime time)
			{
			}
		}
	}
	public class NPCLocationConfig : Config
	{
		public String Key { get; set; }
		public override Config GenerateBaseConfig(Config baseConfig)
		{
			Key = "Z";
			return this;
		}
	}

}
