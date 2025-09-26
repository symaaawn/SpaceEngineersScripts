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
        public class RefineryController
        {
            #region private fields

            private readonly Logger _logger;
            private readonly RefineryManager _refineryManager;
            private readonly IMyBroadcastListener _igcListener;

            #endregion

            #region construction

            public RefineryController(Logger logger, RefineryManager refineryManager, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _refineryManager = refineryManager;
                _igcListener = igc.RegisterBroadcastListener(IgcTagDc.RefineryServiceRequest);
            }

            #endregion
        }
    }
}
