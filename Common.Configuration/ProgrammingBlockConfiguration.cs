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
        public class ProgrammingBlockConfiguration
        {
            #region constants

            private const string _baseServiceConfigurationSection = "BaseConfiguration";
            private const string _shipIdKey = "shipId";

            #endregion

            #region properties

            public string ShipId { get; private set; }

            #endregion

            #region constructors

            public ProgrammingBlockConfiguration(IMyProgrammableBlock programmableBlock, MyIni ini)
            {
                MyIniParseResult result;
                if (!ini.TryParse(programmableBlock.CustomData, out result))
                    throw new Exception(result.ToString());

                ShipId = ini.Get(_baseServiceConfigurationSection, _shipIdKey).ToString();
            }

            #endregion
        }
    }
}
