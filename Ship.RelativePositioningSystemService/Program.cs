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
    partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("RelativePositioningSystemService", "0.1.0", LogLevelDc.Debug);
        private const string ServiceConfigurationSection = "RelativePositioningSystemService";

        #endregion

        #region private fields

        private readonly MyCommandLine _commandLine = new MyCommandLine();
        private readonly Logger _logger = new Logger(ProgramInformation);
        private readonly MyIni _ini = new MyIni();
        private readonly BaseConfiguration _relativePositioningSystemConfiguration;

        private readonly RelativePositioningSystemManager _relativePositioningSystemManager;
        private readonly RelativePositioningSystemActions _relativePositioningSystemActions;
        private readonly RelativePositioningSystemClient _relativePositioningSystemClient;

        #endregion

        public Program()
        {
            _relativePositioningSystemConfiguration = new BaseConfiguration(Me, _ini);

            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            var statusLights = new List<IMyLightingBlock>();
            GridTerminalSystem.GetBlocksOfType(statusLights, statusLight => MyIni.HasSection(statusLight.CustomData, ServiceConfigurationSection));

            var referenceControls = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType(referenceControls, reference => MyIni.HasSection(reference.CustomData, ServiceConfigurationSection));
            var referenceControl = referenceControls.FirstOrDefault();
            if (referenceControl == null)
            {
                _logger.LogFatal("Could not find reference control");
            }

            _relativePositioningSystemClient = new RelativePositioningSystemClient(_logger, _relativePositioningSystemConfiguration, IGC);
            _relativePositioningSystemActions = new RelativePositioningSystemActions(_logger);
            _relativePositioningSystemManager = new RelativePositioningSystemManager(_logger, _relativePositioningSystemActions, _relativePositioningSystemClient, statusLights, referenceControl);

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            _logger.LogInfo("Initialized");
        }

        public void Save()
        {

        }

        public void Main(string argument, UpdateType updateSource)
        {
            _relativePositioningSystemManager.Update();
        }
    }
}
