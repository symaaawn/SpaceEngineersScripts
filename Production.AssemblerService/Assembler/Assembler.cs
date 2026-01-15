using Sandbox.Game.Entities.Cube;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        public class Assembler
        {
            #region private fields

            private readonly IMyAssembler _assembler;
            private readonly AssemblerConfiguration _assemblerConfiguration;

            #endregion

            #region properties

            public string Name => string.IsNullOrEmpty(_assemblerConfiguration.DisplayName)
                ? _assembler.CustomName
                : _assemblerConfiguration.DisplayName;
            public IMyInventory Inventory => _assembler.GetInventory();
            public string Status => _assembler.IsProducing ? "Producing" : "Idle";

            #endregion

            #region construction

            public Assembler(IMyAssembler assembler)
            {
                _assembler = assembler;
                _assemblerConfiguration = new AssemblerConfiguration(_assembler, new MyIni());
            }

            #endregion
        }
    }
}
