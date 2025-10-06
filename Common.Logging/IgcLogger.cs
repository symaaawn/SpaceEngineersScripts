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
        internal class IgcLogger : ILogger
        {
            #region private fields

            private readonly IMyIntergridCommunicationSystem _igcSystem;
            private readonly TransmissionDistance _transmissionDistance;
            private readonly LogLevelDc _logLevel;

            #endregion

            #region construction

            public IgcLogger(IMyIntergridCommunicationSystem igcSystem, TransmissionDistance transmissionDistance = TransmissionDistance.AntennaRelay, LogLevelDc logLevel = LogLevelDc.Debug)
            {
                _igcSystem = igcSystem;
                _transmissionDistance = transmissionDistance;
                _logLevel = logLevel;
            }

            #endregion

            public void Log(string message, LogLevelDc logLevel)
            {
                if (logLevel < _logLevel) return;

                _igcSystem.SendBroadcastMessage(IgcTagDc.LogMessage, message, _transmissionDistance);
            }
        }
    }
}