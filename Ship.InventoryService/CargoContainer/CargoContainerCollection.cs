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
        internal class CargoContainerCollection
        {
            #region private fields
            
            private readonly List<CargoContainer> _cargoContainers;

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

            public CargoContainer GetCargoContainerWithEnoughSpace(MyFixedPoint amount, MyItemType itemType)
            {
                return _cargoContainers.FirstOrDefault(c => c.CanItemsBeAdded(amount, itemType));
            }

            #endregion
        }
    }
}
