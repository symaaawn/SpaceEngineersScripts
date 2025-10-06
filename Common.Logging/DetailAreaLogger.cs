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
        internal class DetailAreaLogger : ILogger
        {
            #region private fields

            private Queue<string> _logHistory;
            private readonly Action<string> _echo;
            private readonly LogLevelDc _logLevel;

            #endregion

            #region construction

            public DetailAreaLogger(Action<string> echoCallback, LogLevelDc logLevel = LogLevelDc.Debug)
            {
                _logHistory = new Queue<string>();
                _echo = echoCallback;
                _logLevel = logLevel;
            }

            #endregion

            public void Log(string message, LogLevelDc logLevel)
            {
                if (logLevel < _logLevel) return;

                _logHistory.Enqueue(message);
                _echo(message);
            }
        }
    }
}