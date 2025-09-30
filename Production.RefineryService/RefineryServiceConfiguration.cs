using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryServiceConfiguration : BaseConfiguration
        {
            #region constants

            private const string _RefineryServiceConfigurationSection = "RefineryServiceConfiguration";
            private const string _refineryBuffer = "refineryBuffer";

            #endregion

            #region properties

            public float RefineryBuffer { get; private set; } = 5f;

            #endregion

            #region constructors

            public RefineryServiceConfiguration(IMyTerminalBlock terminalBlock, MyIni ini) 
                : base(terminalBlock, ini)
            {
                MyIniParseResult result;

                if (!ini.TryParse(terminalBlock.CustomData, out result))
                    throw new Exception(result.ToString());

                RefineryBuffer = ini.Get(_RefineryServiceConfigurationSection, _refineryBuffer).ToSingle(5f);
            }

            #endregion
        }
    }
}
