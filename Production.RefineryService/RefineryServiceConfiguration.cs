using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryServiceConfiguration : BaseConfiguration
        {
            public RefineryServiceConfiguration(IMyTerminalBlock terminalBlock, MyIni ini) : base(terminalBlock, ini)
            {
            }
        }
    }
}
