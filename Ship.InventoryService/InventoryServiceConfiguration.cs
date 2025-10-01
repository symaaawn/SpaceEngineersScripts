using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame.Utilities;

namespace IngameScript
{
    partial class Program
    {
        public class InventoryServiceConfiguration : BaseConfiguration
        {
            #region constants

            private const string _inventoryServiceConfigurationSection = "InventoryServiceConfiguration";

            #endregion

            #region properties


            #endregion

            #region constructors

            public InventoryServiceConfiguration(IMyProgrammableBlock programmableBlock, MyIni ini)
                : base(programmableBlock, ini)
            {
                MyIniParseResult result;

                if (!ini.TryParse(programmableBlock.CustomData, out result))
                    throw new Exception(result.ToString());
            }

            #endregion
        }
    }
}
