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
        public class RefineryClient
        {
            #region private fields

            private readonly string _inventoryRequestTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Request;
            private readonly string _inventoryResponseTag = IgcTagDc.InventoryService + "/" + IgcTagDc.Response;
            private readonly Logger _logger;
            private readonly RefineryServiceConfiguration _refineryServiceConfiguration;
            private readonly IMyIntergridCommunicationSystem _igc;
            private readonly IMyBroadcastListener _igcListener;

            private int _messageCount = 0;

            #endregion

            #region properties

            public List<InventoryServiceMessage> PendingMessages { get; private set; }

            #endregion

            #region construction

            public RefineryClient(Logger logger, RefineryServiceConfiguration refineryServiceConfiguration, IMyIntergridCommunicationSystem igc)
            {
                _logger = logger;

                _refineryServiceConfiguration = refineryServiceConfiguration;
                _inventoryRequestTag = refineryServiceConfiguration.ShipId + "/" + _inventoryRequestTag;
                _inventoryResponseTag = refineryServiceConfiguration.ShipId + "/" + _inventoryResponseTag;

                _igc = igc;
                _igcListener = igc.RegisterBroadcastListener(_inventoryResponseTag);
                _igcListener.SetMessageCallback(_inventoryResponseTag);

                PendingMessages = new List<InventoryServiceMessage>();
            }

            #endregion

            #region methods

            public List<InventoryServiceMessage> GetPendingMessages()
            {
                var messages = PendingMessages.ToList();
                PendingMessages.Clear();
                return messages;
            }

            #endregion

            #region response methods

            public void CheckResponses()
            {
                _logger.LogDebug("Checking for IGC messages");

                while (_igcListener.HasPendingMessage)
                {
                    var rawMessage = _igcListener.AcceptMessage();
                    if (rawMessage.Tag == _inventoryResponseTag)
                    {
                        HandleResponse(rawMessage);
                    }
                }
            }

            public void HandleResponse(MyIGCMessage rawMessage)
            {
                if (rawMessage.Data is ImmutableDictionary<string, string>)
                {
                    var message = new InventoryServiceMessage().Deserialize((ImmutableDictionary<string, string>)rawMessage.Data);
                    _logger.LogDebug($"Received message with method {message.Method}");

                    switch (message.Method)
                    {
                        case "GetInventory":
                            var inventoryMessage = message as InventoryServiceMessage_GetInventory;

                            if (inventoryMessage != null)
                            {
                                _logger.LogInfo($"Received inventory with {inventoryMessage.Inventory.Count} items");

                                PendingMessages.Add(inventoryMessage);
                            }
                            else
                            {
                                _logger.LogWarning("Invalid GetInventory message");
                            }

                            break;
                        default:
                            _logger.LogWarning($"Unknown method '{message.Method}'");
                            break;
                    }
                }
            }

            #endregion

            #region request methods

            public void SendGetInventoryItems()
            {
                var message = new InventoryServiceMessage_GetInventory()
                {
                    RequestId = ++_messageCount,
                    Method = "GetInventory",
                };

                _logger.LogDebug($"Sending GetInventory request: tag={_inventoryRequestTag}");

                foreach (var line in message.Serialize())
                {
                    _logger.LogDebug($"{line.Key}: {line.Value}");
                }

                _igc.SendBroadcastMessage(_inventoryRequestTag, message.Serialize());
            }

            public void SendPullRequest()
            {

            }

            public void SendPushRequest(string item, MyFixedPoint amount, string targetInventory)
            {
                var message = new InventoryServiceMessage_PushItems()
                {
                    RequestId = ++_messageCount,
                    Method = "PushItems",
                    Item = item,
                    Amount = amount,
                    TargetInventory = targetInventory
                };

                _logger.LogDebug($"Sending PushItems request: tag={_inventoryRequestTag}");

                foreach(var line in message.Serialize())
                {
                    _logger.LogDebug($"{line.Key}: {line.Value}");
                }

                _igc.SendBroadcastMessage(_inventoryRequestTag, message.Serialize());
            }

            #endregion
        }
    }
}
