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
    /**
     * <summary>
     * Service that manages assemblers on the same construct.
     * </summary>
     * <remarks>
     * Add the tag <c>AssemblerService</c> to the custom data of the assemblers you want to use.
     * </remarks>
     */
    public partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("AssemblerService", "0.0.1", LogLevelDc.Debug);
        private const string AssemblerServiceTag = "AssemblerService";

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();
        private readonly AssemblerServiceConfiguration _assemblerServiceConfiguration;

        private readonly AssemblerManager _assemblerManager;
        private readonly AssemblerClient _assemblerClient;

        #endregion

        public Program()
        {
            _assemblerServiceConfiguration = new AssemblerServiceConfiguration(Me, _ini);

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));
            _logger.AddLogger(new BroadcastControllerLogger(this));

            _logger.LogDebug("Starting AssemblerService");

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            var statusLights = new List<IMyLightingBlock>();
            GridTerminalSystem.GetBlocksOfType(statusLights, light => MyIni.HasSection(light.CustomData, AssemblerServiceTag));
            if (statusLights.Count == 0)
            {
                _logger.LogWarning($"No status light with tag '{AssemblerServiceTag}' found.");
            }
            else
            {
                _logger.LogInfo($"Found {statusLights.Count} status lights with tag '{AssemblerServiceTag}'.");
            }

            var displays = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(displays, display => display.CustomData.Contains(AssemblerServiceTag));
            if (displays.Count == 0)
            {
                _logger.LogWarning("No displays found with the AssemblerService tag in CustomData.");
            }

            var assemblers = new List<IMyAssembler>();
            GridTerminalSystem.GetBlocksOfType(assemblers, assembler => assembler.CustomData.Contains(AssemblerServiceTag));
            if (assemblers.Count == 0)
            {
                _logger.LogWarning("No assemblers found with the AssemblerService tag in CustomData.");
            }

            _assemblerManager = new AssemblerManager(_logger, GridTerminalSystem, statusLights, displays, assemblers);
            _assemblerClient = new AssemblerClient(_logger, _assemblerServiceConfiguration, IGC);
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateType)
        {
            if ((updateType & UpdateType.IGC) != 0)
            {
                _logger.LogInfo("Processing IGC messages");
                _assemblerClient.CheckResponses();
            }

            if ((updateType & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) != 0)
            {
                _logger.LogInfo("Processing inventory updates");
                _assemblerManager.Update();
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
                _assemblerManager.SetServiceState(state);
            }
        }
    }
}
