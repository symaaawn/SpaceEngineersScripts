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
        public class AssemblerServiceConfiguration : BaseConfiguration
        {
            #region constants

            private const string _AssemblerServiceConfigurationSection = "AssemblerServiceConfiguration";

            #endregion

            #region properties

            #endregion

            #region constructors

            public AssemblerServiceConfiguration(IMyTerminalBlock terminalBlock, MyIni ini)
                : base(terminalBlock, ini)
            {
                MyIniParseResult result;
                if (!ini.TryParse(terminalBlock.CustomData, out result))
                    throw new Exception(result.ToString());
            }

            #endregion
        }
    }
}
