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
        public class CargoContainer
        {
            #region private fields

            private readonly IMyCargoContainer _cargoContainer;
            private readonly CargoContainerConfiguration _cargoContainerConfiguration;

            #endregion

            #region properties

            public string Name => string.IsNullOrEmpty(_cargoContainerConfiguration.DisplayName)
                ? _cargoContainer.CustomName
                : _cargoContainerConfiguration.DisplayName;
            public CargoContainerTypeDc CargoContainerType => _cargoContainerConfiguration.CargoContainerType;
            public int FillPriority => _cargoContainerConfiguration.FillPriority;
            public IMyInventory Inventory => _cargoContainer.GetInventory();

            #endregion

            #region construction

            public CargoContainer(IMyCargoContainer cargoContainer)
            {
                _cargoContainer = cargoContainer;
                _cargoContainerConfiguration = new CargoContainerConfiguration(_cargoContainer, new MyIni());
            }

            #endregion

            #region methods

            public bool CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType)
            {
                return _cargoContainer.GetInventory().CanItemsBeAdded(amount, itemType);
            }

            public List<MyInventoryItem> GetItems()
            {
                var items = new List<MyInventoryItem>();
                _cargoContainer.GetInventory().GetItems(items);
                return items;
            }

            #endregion
        }
    }
}
