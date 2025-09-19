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
        public class InventoryController
        {
            #region private fields

            private readonly Logger _logger;
            private readonly InventoryManager _inventoryManager;
            private readonly IMyBroadcastListener _igcListener;

            #endregion

            #region construction

            public InventoryController(Logger logger, InventoryManager inventoryManager, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _inventoryManager = inventoryManager;

                _igcListener = igc.RegisterBroadcastListener(IgcTagDc.InventoryServiceRequest);
            }

            #endregion

            #region methods

            public void Update()
            {
                while (_igcListener.HasPendingMessage)
                {
                    var message = _igcListener.AcceptMessage();
                    if (message.Tag == IgcTagDc.InventoryServiceRequest)
                    {
                        HandleRequest(message);
                    }
                }
            }

            public void HandleRequest(MyIGCMessage message)
            {
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var parts = message.Data.ToString().Split(';');
            }

            #endregion
        }
    }
}
