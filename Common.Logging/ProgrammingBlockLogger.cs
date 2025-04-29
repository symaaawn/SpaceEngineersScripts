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
        internal class ProgrammingBlockLogger : ILogger
        {
            #region constants

            private const int CharacterHeight = 20;

            #endregion

            #region private fields

            private readonly TextPanelDisplay _programmingBlockDisplay;
            private readonly FixedLengthQueue<string> _logHistory;

            #endregion

            #region construction

            public ProgrammingBlockLogger(IMyProgrammableBlock programmableBlock)
            {
                _programmingBlockDisplay = new TextPanelDisplay(programmableBlock.GetSurface(0));
                var lines = (int)(_programmingBlockDisplay.Viewport.Height / CharacterHeight) - 3;
                _logHistory = new FixedLengthQueue<string>(lines);
            }

            #endregion

            public void Log(string message)
            {
                _logHistory.Enqueue(message);
                _programmingBlockDisplay.RenderDisplay(_logHistory.ToList());
            }
        }
    }
}