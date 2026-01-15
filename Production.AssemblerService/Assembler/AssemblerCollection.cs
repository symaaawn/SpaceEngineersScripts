using Sandbox.Game;
using Sandbox.Game.Entities;
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
        public class AssemblerCollection
        {
            #region private fields

            private readonly Logger _logger;
            private List<Assembler> _assemblers = new List<Assembler>();

            private List<MyInventoryItem> _tempMyInventoryItems = new List<MyInventoryItem>();
            private Dictionary<string, MyFixedPoint> _itemCache = new Dictionary<string, MyFixedPoint>();

            #endregion

            #region construction

            public AssemblerCollection(Logger logger, List<IMyAssembler> assemblers)
            {
                _logger = logger;

                foreach (var assembler in assemblers)
                {
                    _assemblers.Add(new Assembler(assembler));
                }
            }

            #endregion

            #region methods

            public int AssemblerCount => _assemblers.Count;

            public List<Assembler> GetAssemblers()
            {
                return _assemblers;
            }

            #endregion
        }
    }
}
