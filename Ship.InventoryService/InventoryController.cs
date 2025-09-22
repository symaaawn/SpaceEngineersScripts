using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
                    var rawMessage = _igcListener.AcceptMessage();
                    if (rawMessage.Tag == IgcTagDc.InventoryServiceRequest)
                    {
                        HandleRequest(rawMessage);
                    }
                }
            }

            public void HandleRequest(MyIGCMessage rawMessage)
            {
                if(rawMessage.Data is ImmutableDictionary<string, string>)
                {
                    var message = new InventoryServiceMessage().Deserialize((ImmutableDictionary<string, string>)rawMessage.Data);

                    switch (message.Method)
                    {
                        case "PullItems":
                            var pullMessage = message as InventoryServiceMessage_PullItems;
                            if(pullMessage != null)
                            {
                                _inventoryManager.PullItems(pullMessage);
                            }
                            else
                            {
                                _logger.LogError("Invalid PullItems message");
                            }
                            break;
                        case "PushItems":
                            var pushMessage = message as InventoryServiceMessage_PushItems;
                            if(pushMessage != null)
                            {
                                _inventoryManager.PushItems(pushMessage);
                            }
                            else
                            {
                                _logger.LogError("Invalid PushItems message");
                            }
                            break;
                        default:
                            _logger.LogError($"Unknown method: {message.Method}");
                            break;
                    }
                }
                else
                {
                    _logger.LogError("Invalid message data");
                }
            }

            #endregion
        }
    }
}
