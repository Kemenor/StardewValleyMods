using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm;
using Storm.ExternalEvent;
using Storm.StardewValley.Wrapper;
using Storm.StardewValley.Event;
using Storm.StardewValley.Accessor.Terrain;
using System.Runtime.InteropServices;
using System.Threading;
using Storm.StardewValley.Accessor;
using System.IO;
using Storm.StardewValley.Proxy;
using Storm.ExternalEvent;
using Storm.StardewValley;
using Storm.StardewValley.Accessor;
using Storm.StardewValley.Event;
using Storm.StardewValley.Proxy;
using Storm.StardewValley.Wrapper;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace UsefullSprinklers
{
    [Mod]
    public class UsefullSprinklers : DiskResource
    {
        public static SprinklerConfig SConfig { get; private set; }
        [Subscribe]
        public void Init(InitializeEvent @event)
        {
            SConfig = new SprinklerConfig();
            SConfig = (SprinklerConfig)Config.InitializeConfig(Config.GetBasePath(this), SConfig);
        }


        [Subscribe]
        public void PreObjectDayUpdateCallback(PreObjectDayUpdateEvent @event)
        {
            ObjectItem obj = @event.This;
            GameLocation location = @event.ArgLocation;
            switch (obj.Name)
            {
                case "Sprinkler":
                    waterArea(obj, location, SConfig.Sprinkler);
                    break;
                case "Quality Sprinkler":
                    waterArea(obj, location, SConfig.QualitySprinkler);
                    break;
                case "Iridium Sprinkler":
                    waterArea(obj, location, SConfig.IridiumSprinkler);
                    break;
            }

            @event.ReturnEarly = true;
        }
        private void waterArea(ObjectItem obj, GameLocation location, int area)
        {
            if (area % 2 == 0)
            {
                area++;
            }
            Microsoft.Xna.Framework.Vector2 tileLocation = obj.TileLocation;
            tileLocation.X -= area / 2;
            tileLocation.Y -= area / 2;
            for (int i = 0; i < area; i++)
            {
                for (int j = 0; j < area; j++)
                {
                    if (location.TerrainFeatures.ContainsKey(tileLocation))
                    {
                        if (location.TerrainFeatures[tileLocation].Is<HoeDirtAccessor>())
                        {
                            HoeDirt hoeDirtObj = location.TerrainFeatures[tileLocation].As<HoeDirtAccessor, HoeDirt>();
                            hoeDirtObj.State = 1;
                        }
                    }
                    tileLocation.X += 1;
                }
                tileLocation.X -= area;
                tileLocation.Y += 1;
            }
        }
    }

    public class SprinklerConfig : Config
    {
        public int Sprinkler { get; set; }
        public int QualitySprinkler { get; set; }
        public int IridiumSprinkler { get; set; }

        public override Config GenerateBaseConfig(Config baseConfig)
        {
            Sprinkler = 3;
            QualitySprinkler = 5;
            IridiumSprinkler = 7;

            return this;
        }
    }
}
