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

            public CargoContainerTypeDc CargoContainerType => _cargoContainerConfiguration.CargoContainerType;
            public int FillPriority => _cargoContainerConfiguration.FillPriority;
            public List<InventoryItem> Items { get; private set; } = new List<InventoryItem>();

            #endregion

            #region construction

            public CargoContainer(IMyCargoContainer cargoContainer)
            {
                _cargoContainer = cargoContainer;
                _cargoContainerConfiguration = new CargoContainerConfiguration(_cargoContainer, new MyIni());

                var items = new List<MyInventoryItem>();
                _cargoContainer.GetInventory().GetItems(items);
                foreach (var item in items)
                {
                    Items.Add(new InventoryItem(item, cargoContainer.GetInventory()));
                }
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
