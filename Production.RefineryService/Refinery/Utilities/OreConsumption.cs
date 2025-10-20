using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.GameServices;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        public class OreConsumption
        {
            #region properties

            public string Type { get; }
            public int KgPerHour { get; }
            public float ConversionRatio { get; }
            public float KgPerMinute => KgPerHour / 60f;
            public float KgPerSecond => KgPerHour / 3600f;
            public float SecondsPerKg => 3600f / KgPerHour;


            #endregion

            #region construction

            public OreConsumption(string type, int kgPerHour, float conversionRatio)
            {
                Type = type;
                KgPerHour = kgPerHour;
                ConversionRatio = conversionRatio;
            }

            #endregion

            public double Priority()
            {
                return Math.Pow(1.0 / ConversionRatio, 0.01);
            }
        }

        public static class OreConsumptions
        {
            public static readonly OreConsumption Stone     =   new OreConsumption("Stone",     468000, 0.03f);
            public static readonly OreConsumption Scrap     =   new OreConsumption("Scrap",     117000, 0.8f);
            public static readonly OreConsumption Iron      =   new OreConsumption("Iron",       93600, 0.7f);
            public static readonly OreConsumption Gold      =   new OreConsumption("Gold",       11700, 0.01f);
            public static readonly OreConsumption Nickel    =   new OreConsumption("Nickel",      7091, 0.4f);
            public static readonly OreConsumption Cobalt    =   new OreConsumption("Cobalt",      1560, 0.3f);
            public static readonly OreConsumption Silicon   =   new OreConsumption("Silicon",     7800, 0.7f);
            public static readonly OreConsumption Magnesium =   new OreConsumption("Magnesium",   9360, 0.007f);
            public static readonly OreConsumption Silver    =   new OreConsumption("Silver",      4680, 0.1f);
            public static readonly OreConsumption Platinum  =   new OreConsumption("Platinum",    1560, 0.005f);
            public static readonly OreConsumption Uranium   =   new OreConsumption("Uranium",     1170, 0.01f);

            public static OreConsumption GetOreConsumption(string name)
            {
                switch (name)
                {
                    case "Stone":
                        return Stone;
                    case "Scrap":
                        return Scrap;
                    case "Iron":
                        return Iron;
                    case "Nickel":
                        return Nickel;
                    case "Cobalt":
                        return Cobalt;
                    case "Silicon":
                        return Silicon;
                    case "Magnesium":
                        return Magnesium;
                    case "Gold":
                        return Gold;
                    case "Silver":
                        return Silver;
                    case "Platinum":
                        return Platinum;
                    case "Uranium":
                        return Uranium;
                    default:
                        throw new ArgumentException($"Unknown ore type: {name}");
                }
            }
        }
    }
}
