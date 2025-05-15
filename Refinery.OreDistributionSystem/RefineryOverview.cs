﻿using Sandbox.Game.EntityComponents;
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
        public class RefineryOverview
        {
            #region properties

            public IMyRefinery Refinery { get; set; }

            public float RefineSpeed { get; set; }
            public float YieldRate { get; set; }
            public float PowerEfficiency { get; set; }

            #endregion

            public RefineryOverview(IMyRefinery refinery)
            {
                Refinery = refinery;
                var detailedInfo = RefineryDetailedInfoParser.Parse(Refinery.DetailedInfo);
                RefineSpeed = detailedInfo.RefineSpeed;
                YieldRate = detailedInfo.YieldRate;
                PowerEfficiency = detailedInfo.PowerEfficiency;
            }
        }
    }
}
