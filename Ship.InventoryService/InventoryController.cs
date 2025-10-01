using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;

namespace IngameScript
{
    partial class Program
    {
        public class InventoryController
        {
            #region constants

            #endregion

            #region private fields

            private readonly string _inventoryRequestTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Request;
            private readonly string _inventoryResponseTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Response;
            private readonly Logger _logger;
            private readonly InventoryManager _inventoryManager;
            private readonly IMyIntergridCommunicationSystem _igc;
            private readonly IMyBroadcastListener _igcListener;
            private readonly InventoryServiceConfiguration _inventoryServiceConfiguration;

            #endregion

            #region construction

            public InventoryController(Logger logger, InventoryServiceConfiguration inventoryServiceConfiguration, InventoryManager inventoryManager, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;
                _inventoryManager = inventoryManager;

                _inventoryServiceConfiguration = inventoryServiceConfiguration;
                _inventoryRequestTag = inventoryServiceConfiguration.ShipId + "/" + _inventoryRequestTag;   
                _inventoryResponseTag = inventoryServiceConfiguration.ShipId + "/" + _inventoryResponseTag;

                _igc = igc;
                _igcListener = igc.RegisterBroadcastListener(_inventoryRequestTag);
                _igcListener.SetMessageCallback(_inventoryRequestTag);
            }

            #endregion

            #region methods

            public void CheckRequests()
            {
                _logger.LogDebug("Checking for IGC messages");

                while (_igcListener.HasPendingMessage)
                {
                    var rawMessage = _igcListener.AcceptMessage();
                    if (rawMessage.Tag == _inventoryRequestTag)
                    {
                        HandleRequest(rawMessage);
                    }
                }
            }

            public void HandleRequest(MyIGCMessage rawMessage)
            {
                if (rawMessage.Data is ImmutableDictionary<string, string>)
                {
                    var message = new InventoryServiceMessage().Deserialize((ImmutableDictionary<string, string>)rawMessage.Data);
                    _logger.LogDebug($"Received message with method {message.Method}");

                    switch (message.Method)
                    {
                        case "GetInventory":
                            var getMessage = message as InventoryServiceMessage_GetInventory;

                            if(getMessage != null)
                            {
                                _logger.LogDebug($"Received GetInventory request");
                                var inventory = _inventoryManager.GetInventory();

                                var inventoryDict = new Dictionary<string, MyFixedPoint>();
                                foreach (var item in inventory)
                                {
                                    inventoryDict.Add(item.Type.ToString(), item.Amount);
                                }

                                var responseMessage = new InventoryServiceMessage_GetInventory()
                                {
                                    RequestId = getMessage.RequestId,
                                    Method = "GetInventory",
                                    Inventory = inventoryDict
                                };

                                _logger.LogDebug($"Sending GetInventory response: {string.Join(", ", inventoryDict.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");
                                _igc.SendBroadcastMessage(_inventoryResponseTag, responseMessage.Serialize());
                            }
                            else
                            {
                                _logger.LogError("Invalid GetInventory message");
                            }
                            break;

                        case "PullItems":
                            var pullMessage = message as InventoryServiceMessage_PullItems;

                            if(pullMessage != null)
                            {
                                _logger.LogDebug($"Received PullItems request: Item={pullMessage.Item}, Amount={pullMessage.Amount}, SourceInventory={pullMessage.SourceInventory}");
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
                                _logger.LogDebug($"Received PushItems request: Item={pushMessage.Item}, Amount={pushMessage.Amount}, TargetInventory={pushMessage.TargetInventory}");
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
