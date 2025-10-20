using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game.ModAPI.Ingame;
using static IngameScript.Program;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryCollection
        {
            #region private fields

            private readonly List<Refinery> _refineries;

            #endregion

            #region construction

            public RefineryCollection(List<IMyRefinery> refineries)
            {
                _refineries = new List<Refinery>();
                foreach (var refinery in refineries)
                {
                    _refineries.Add(new Refinery(refinery));
                }
            }

            #endregion

            #region methods

            public int RefineryCount => _refineries.Count;

            public List<Refinery> GetRefineries()
            {
                return _refineries;
            }

            public Refinery GetRefineryByName(string name)
            {
                return _refineries.FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }

            public List<Refinery> GetIdleRefineries(float buffer = 5f)
            {
                var idleRefineries = new List<Refinery>();

                var refineryOres = new List<MyInventoryItem>();
                foreach (var refinery in _refineries)
                {
                    refinery.InputInventory.GetItems(refineryOres);
                    if (refineryOres.All(o => o.Amount < (MyFixedPoint)(OreConsumptions.GetOreConsumption(o.Type.SubtypeId).KgPerSecond * buffer)))
                    {
                        idleRefineries.Add(refinery);
                    }
                }

                return idleRefineries;
            }

            #endregion
        }
    }
}
