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

        #endregion

        public Program()
        {
            _refineryServiceConfiguration = new RefineryServiceConfiguration(Me, _ini);

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this, LogLevelDc.Warning));

            _logger.LogDebug($"Starting RefineryService...");

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            var statusLights = new List<IMyLightingBlock>();
            GridTerminalSystem.GetBlocksOfType(statusLights, light => MyIni.HasSection(light.CustomData, RefineryServiceTag));
            if (statusLights.Count == 0)
            {
                _logger.LogWarning($"No status light with tag '{RefineryServiceTag}' found.");
            }
            else
            {
                _logger.LogInfo($"Found {statusLights.Count} status lights with tag '{RefineryServiceTag}'.");
            }

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

            var displays = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(displays, display => MyIni.HasSection(display.CustomData, RefineryServiceTag));
            if (displays.Count == 0)
            {
                _logger.LogWarning($"No display with tag '{RefineryServiceTag}' found.");
            }
            else
            {
                _logger.LogInfo($"Found {displays.Count} displays with tag '{RefineryServiceTag}'.");
            }

            _refineryClient = new RefineryClient(_logger, _refineryServiceConfiguration, IGC);
            _refineryActions = new RefineryActions(_logger, GridTerminalSystem);
            _refineryManager = new RefineryManager(_logger, _refineryServiceConfiguration, _refineryActions, _refineryClient, statusLights, displays, refineries);
            _refineryController = new RefineryController(_logger, _refineryServiceConfiguration, _refineryManager, IGC);

            _logger.LogInfo($"RefineryService started.");
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) != 0)
            {
                _logger.LogDebug("Processing IGC messages");
                _refineryClient.CheckResponses();
                _refineryManager.ProcessInventoryResponse();
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                _logger.LogDebug($"Main");
                _refineryManager.Update();
            }

            if ((updateType & (UpdateType.Trigger | UpdateType.Terminal)) != 0)
            {
                _logger.LogDebug($"Processing argument: {argument}");
                ServiceStateDc state;
                if (!Enum.TryParse(argument?.Trim(), true, out state))
                {
                    _logger.LogError($"Invalid service state: {argument}");
                    return;
                }
                _refineryManager.SetServiceState(state);
            }
        }
    }
}
