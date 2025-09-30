using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program 
    {
        public class CargoContainerCollection
        {
            #region private fields
            
            private List<CargoContainer> _cargoContainers;

            #endregion

            #region construction

            public CargoContainerCollection(List<IMyCargoContainer> cargoContainers)
            {
                _cargoContainers = new List<CargoContainer>();
                foreach (var cargoContainer in cargoContainers)
                {
                    _cargoContainers.Add(new CargoContainer(cargoContainer));
                }
                _cargoContainers = _cargoContainers.OrderBy(c => c.FillPriority).ToList();
            }

            #endregion

            #region methods

            public int ContainerCount => _cargoContainers.Count;
            public int ItemCount => _cargoContainers.Select(c => c.Items).Count();

            public void UpdateCollection(List<IMyCargoContainer> cargoContainers)
            {
                _cargoContainers.Clear();
                foreach (var cargoContainer in cargoContainers)
                {
                    _cargoContainers.Add(new CargoContainer(cargoContainer));
                }
                _cargoContainers = _cargoContainers.OrderBy(c => c.FillPriority).ToList();
            }

            public IMyInventory[] GetInventories()
            {
                return _cargoContainers.Select(c => c.Inventory).ToArray();
            }

            public CargoContainer GetCargoContainerByName(string name)
            {
                return _cargoContainers.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }

            public CargoContainer GetCargoContainerWithItem(MyItemType itemType, MyFixedPoint amount)
            {
                return _cargoContainers.FirstOrDefault(c => c.Items.Any(i => i.Item.Type == itemType && i.Item.Amount >= amount));
            }

            public CargoContainer GetCargoContainerWithEnoughSpace(MyItemType itemType, MyFixedPoint amount)
            {
                return _cargoContainers.FirstOrDefault(c => c.CanItemsBeAdded(amount, itemType));
            }

            #endregion
        }
    }
}
