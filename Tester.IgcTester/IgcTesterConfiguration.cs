using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame.Utilities;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program
    {
        public class IgcTesterConfiguration : BaseConfiguration
        {
            #region constants

            private const string _igcTesterConfigurationSection = "IgcTesterConfiguration";
            private const string _listenerTagKey = "listenerTag";

            #endregion

            #region properties

            public string ListenerTag { get; private set; }

            #endregion

            #region constructors

            public IgcTesterConfiguration(IMyTerminalBlock terminalBlock, MyIni ini)
                : base(terminalBlock, ini)
            {
                MyIniParseResult result;

                if (!ini.TryParse(terminalBlock.CustomData, out result))
                    throw new Exception(result.ToString());

                ListenerTag = ShipId + "/" + ini.Get(_igcTesterConfigurationSection, _listenerTagKey).ToString();
            }

            #endregion
        }
    }
}
