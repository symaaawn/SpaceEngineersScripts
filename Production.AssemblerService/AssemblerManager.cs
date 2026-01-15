using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace IngameScript
{
    partial class Program
    {
        public class AssemblerManager
        {
            #region private fields

            private ServiceStateDc serviceState;

            private readonly Logger _logger;
            private readonly IMyGridTerminalSystem _gridTerminalSystem;

            #endregion

            #region properties

            private StatusLightCollection StatusLightCollection { get; set; }
            private AssemblerServiceDisplayCollection AssemblerServiceDisplayCollection { get; set; }
            private AssemblerCollection AssemblerCollection { get; set; }

            #endregion

            #region construction

            public AssemblerManager(Logger logger, IMyGridTerminalSystem gridTerminalSystem, List<IMyLightingBlock> statusLights, List<IMyTextPanel> displays, List<IMyAssembler> assemblers)
            {
                _logger = logger;
                _gridTerminalSystem = gridTerminalSystem;

                StatusLightCollection = new StatusLightCollection(statusLights);
                AssemblerServiceDisplayCollection = new AssemblerServiceDisplayCollection(displays);
                AssemblerCollection = new AssemblerCollection(logger, assemblers);

                serviceState = ServiceStateDc.Auto;

                _logger.LogInfo($"Initialized AssemblerManager with {AssemblerCollection.AssemblerCount} assemblers.");
            }

            #endregion

            #region methods

            public void Update()
            {
                switch (serviceState)
                {
                    case ServiceStateDc.Error:
                        break;
                    case ServiceStateDc.Manual:
                        break;
                    case ServiceStateDc.Auto:
                        break;
                }

                AssemblerServiceDisplayCollection.UpdateDisplays(AssemblerCollection, serviceState);

                if (serviceState == ServiceStateDc.Auto)
                {
                    EmptyAssemblerOutputInventories();
                }
            }

            public bool SetServiceState(ServiceStateDc newState)
            {
                if (serviceState != newState)
                {
                    serviceState = newState;
                    _logger.LogInfo($"Refinery service state changed to {serviceState}");
                    StatusLightCollection.UpdateLights(serviceState);

                    return true;
                }

                return false;
            }

            public void EmptyAssemblerOutputInventories()
            {
            }

            #endregion
        }
    }
}