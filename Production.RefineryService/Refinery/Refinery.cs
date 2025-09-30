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
    partial class Program
    {
        public class Refinery
        {
            #region private fields

            private readonly IMyRefinery _refinery;

            #endregion

            #region properties

            public float RefineSpeed { get; set; }
            public float YieldRate { get; set; }
            public float PowerEfficiency { get; set; }

            public string Name => _refinery.CustomName;
            public IMyInventory InputInventory => _refinery.InputInventory;

            #endregion

            #region construction

            public Refinery(IMyRefinery refinery)
            {
                _refinery = refinery;
                var detailedInfo = RefineryDetailedInfoParser.Parse(_refinery.DetailedInfo);
                RefineSpeed = detailedInfo.RefineSpeed;
                YieldRate = detailedInfo.YieldRate;
                PowerEfficiency = detailedInfo.PowerEfficiency;
            }

            #endregion
        }
    }
}
