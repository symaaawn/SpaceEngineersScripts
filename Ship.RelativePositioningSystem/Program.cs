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
     * Relative Positioning System
     * </summary>
     * <remarks>
     * This script is responsible for broadcasting the current position of the reference remote control.
     * Drones and other ships can use this information to calculate their relative position to the reference point.
     * </remarks>
     */
    public partial class Program : MyGridProgram
    {
        #region constants

        private static readonly ProgramInformationDc ProgramInformation = new ProgramInformationDc("RelativePositioningSystem", "0.0.1", LogLevelDc.Debug);

        #endregion

        #region private fields

        private readonly Logger _logger = new Logger(ProgramInformation);

        #endregion

        #region properties

        public List<IMyLightingBlock> StatusLights { get; private set; }
        public IMyRemoteControl ReferenceControl { get; private set; }

        #endregion

        public Program()
        {
            _logger.AddLogger(new DetailAreaLogger(Echo));
            _logger.AddLogger(new ProgrammingBlockLogger(Me));

            var referenceControls = new List<IMyRemoteControl>();
            GridTerminalSystem.GetBlocksOfType(referenceControls, referenceControl => MyIni.HasSection(referenceControl.CustomData, "reference"));
            ReferenceControl = referenceControls.FirstOrDefault();
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save()
        {
        }

        public void Main(string argument, UpdateType updateSource)
        {
            if (ReferenceControl != null && !ReferenceControl.Closed)
            {
                var currentPosition = ReferenceControl.WorldMatrix;
                _logger.LogInfo($"Current position X: {currentPosition.GetRow(3).X} - Y: {currentPosition.GetRow(3).Y} - Z: {currentPosition.GetRow(3).Z}");
                _logger.LogInfo($"{ReferenceControl.WorldMatrix}");

                IGC.SendBroadcastMessage(IgcTagDc.RpsMessage, currentPosition);
            }
            else
            {
                _logger.LogFatal($"Reference remote control not found!");
            }
        }
    }
}
