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

            private readonly string _refineryRequestTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Request;
            private readonly string _refineryResponseTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Response;
            private readonly Logger _logger;
            private readonly RefineryManager _refineryManager;
            private readonly IMyBroadcastListener _igcListener;
            private readonly RefineryServiceConfiguration _refineryServiceConfiguration;

            #endregion

            #region construction

            public RefineryController(Logger logger, RefineryServiceConfiguration refineryServiceConfiguration, RefineryManager refineryManager, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _refineryManager = refineryManager;

                _refineryServiceConfiguration = refineryServiceConfiguration;
                _refineryRequestTag = refineryServiceConfiguration.ShipId + "/" + _refineryRequestTag;
                _refineryResponseTag = refineryServiceConfiguration.ShipId + "/" + _refineryResponseTag;

                _igcListener = igc.RegisterBroadcastListener(IgcTagDc.RefineryServiceMessage);
                _igcListener.SetMessageCallback(_refineryRequestTag);

                _logger.LogInfo("Initialized RefineryController");
            }

            #endregion
        }
    }
}
