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

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("RefineryService", "0.1.0", LogLevelDc.Debug);
        private const string RefineryServiceTag = "RefineryService";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();

        private readonly RefineryServiceConfiguration _refineryServiceConfiguration;

        private readonly RefineryManager _refineryManager;
        private readonly RefineryActions _refineryActions;
        private readonly RefineryController _refineryController;
        private readonly RefineryClient _refineryClient;

        #endregion

        #region properties

        public List<IMyInventory> SourceInventories { get; set; } = new List<IMyInventory>();

        #endregion

        public Program()
        {
            _refineryServiceConfiguration = new RefineryServiceConfiguration(Me, _ini);

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            _logger.LogDebug($"Starting RefineryService...");

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            var refineries = new List<IMyRefinery>();
            GridTerminalSystem.GetBlocksOfType(refineries, refinery => MyIni.HasSection(refinery.CustomData, RefineryServiceTag));
            if (refineries.Count == 0)
            {
                _logger.LogFatal($"No refinery with tag '{RefineryServiceTag}' found.");
            }
            else
            {
                _logger.LogInfo($"Found {refineries.Count} cargo containers with tag '{RefineryServiceTag}'.");
            }

            _refineryClient = new RefineryClient(_logger, _refineryServiceConfiguration, IGC);
            _logger.LogDebug("Initialized RefineryClient");

            _refineryActions = new RefineryActions(_logger, GridTerminalSystem);
            _logger.LogDebug("Initialized RefineryActions");

            _refineryManager = new RefineryManager(_logger, _refineryActions, _refineryClient, refineries);
            _logger.LogDebug("Initialized RefineryManager");

            _refineryController = new RefineryController(_logger, _refineryServiceConfiguration, _refineryManager, IGC);
            _logger.LogDebug("Initialized RefineryController");
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) != 0)
            {
                _logger.LogInfo("Processing IGC messages");
                _refineryClient.CheckResponses();
                _refineryManager.ProcessInventoryResponse();
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                _logger.LogDebug($"Main");
                _refineryManager.Update();
            }
        }
    }
}
