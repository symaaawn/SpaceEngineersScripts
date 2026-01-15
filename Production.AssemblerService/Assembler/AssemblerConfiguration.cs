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
        public class AssemblerConfiguration : BaseConfiguration
        {
            #region constants

            private const string _assemblerConfigurationSection = "AssemblerConfiguration";

            #endregion

            #region properties

            #endregion

            #region constructors

            public AssemblerConfiguration(IMyTerminalBlock terminalBlock, MyIni ini) : base(terminalBlock, ini)
            {
                MyIniParseResult result;

                if (!ini.TryParse(terminalBlock.CustomData, out result))
                    throw new Exception(result.ToString());
            }

            #endregion
        }
    }
}
