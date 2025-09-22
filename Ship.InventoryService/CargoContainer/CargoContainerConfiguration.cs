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
        public class CargoContainerConfiguration : BaseConfiguration
        {
            #region constants

            private const string _cargoContainerConfigurationSection = "CargoContainerConfiguration";
            private const string _cargoContainerTypeKey = "type";
            private const string _fillPriorityKey = "fillPriority";

            #endregion

            #region properties

            public CargoContainerTypeDc CargoContainerType { get; private set; }
            public int FillPriority { get; private set; }

            #endregion

            #region constructors

            public CargoContainerConfiguration(IMyTerminalBlock terminalBlock, MyIni ini) : base(terminalBlock, ini)
            {
                MyIniParseResult result;

                if (!ini.TryParse(terminalBlock.CustomData, out result))
                    throw new Exception(result.ToString());

                CargoContainerType = (CargoContainerTypeDc)Enum.Parse(typeof(CargoContainerTypeDc), ini.Get(_cargoContainerConfigurationSection, _cargoContainerTypeKey).ToString(), true);
                FillPriority = ini.Get(_cargoContainerConfigurationSection, _fillPriorityKey).ToInt32();
            }

            #endregion
        }
    }
}
