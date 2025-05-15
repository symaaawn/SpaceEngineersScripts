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
        public class ProgramInformationDc
        {
            #region properties

            public string Name { get; set; }
            public string Version { get; set; }
            public LogLevelDc LogLevel { get; set; }

            #endregion

            #region construction

            public ProgramInformationDc(string name, string version, LogLevelDc logLevel = LogLevelDc.Info)
            {
                Name = name;
                Version = version;
                LogLevel = logLevel;
            }

            #endregion
        }
    }
}