using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    partial class Program
    {
        public class RefineryManager
        {
            #region private fields

            private readonly Logger _logger;
            private readonly RefineryActions _refineryActions;
            private readonly RefineryClient _refineryClient;

            #endregion

            #region properties

            private RefineryCollection RefineryCollection { get; set; }

            #endregion

            #region construction

            public RefineryManager(Logger logger, RefineryActions refineryActions, RefineryClient refineryClient, List<IMyRefinery> refineries)
            {
                _logger = logger;
                _refineryActions = refineryActions;
                _refineryClient = refineryClient;
                RefineryCollection = new RefineryCollection(refineries);
            }

            #endregion

            #region methods

            public void Update()
            {
                var idleRefineries = RefineryCollection.GetIdleRefineries();
            }

            #endregion
        }
    }
}
