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
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        /**
         * <summary>
         * Very basic data class for parsing the detailed info string from a refinery and storing upgrade module information.
         * </summary>
         */
        public static class RefineryDetailedInfoParser
        {
            public static RefineryDetailedInfoDc Parse(string detailedInfo)
            {
                var lines = detailedInfo.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var info = new RefineryDetailedInfoDc();
                foreach (var line in lines)
                {
                    var parts = line.Split(':');

                    switch (parts[0].Trim())
                    {
                        case "Refine Speed":
                            float refineSpeed;
                            if (float.TryParse(parts[1].Trim().Trim('%'), out refineSpeed))
                            {
                                info.RefineSpeed = refineSpeed / 100f;
                            }
                            break;

                        case "Yield Rate":
                            float yieldRate;
                            if (float.TryParse(parts[1].Trim().Trim('%'), out yieldRate))
                            {
                                info.YieldRate = yieldRate / 100f;
                            }
                            break;

                        case "Power Efficiency":
                            float powerEfficiency;
                            if (float.TryParse(parts[1].Trim().Trim('%'), out powerEfficiency))
                            {
                                info.PowerEfficiency = powerEfficiency / 100f;
                            }
                            break;
                    }
                }
                return info;
            }
        }
    }
}
