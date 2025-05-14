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
        #region constants

        private const string ProgramName = "OreDistributionSystem";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger();
        private readonly MyIni _ini = new MyIni();

        #endregion

        #region properties

        private List<RefineryOverview> Refineries { get; set; } = new List<RefineryOverview>();

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            var broadcastControllers = new List<IMyBroadcastController>();
            GridTerminalSystem.GetBlocksOfType(broadcastControllers);
            if (broadcastControllers.Count > 0)
            {
                _logger.AddLogger(new BroadcastControllerLogger(broadcastControllers.FirstOrDefault()));
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            var refineries = new List<IMyRefinery>();
            GridTerminalSystem.GetBlocksOfType(refineries, refinery => MyIni.HasSection(refinery.CustomData, "oreDistributionSystem"));
            foreach (var refinery in refineries)
            {
                Refineries.Add(new RefineryOverview(refinery));
            }

            _logger.LogDebug($"Found {Refineries.Count} refineries");
            foreach (var refinery in Refineries)
            {
                _logger.LogDebug($"{refinery.Refinery.CustomName}: Speed {refinery.RefineSpeed} / Yield {refinery.YieldRate} / Efficiency {refinery.PowerEfficiency}");
            }
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
            }
        }
    }
}
