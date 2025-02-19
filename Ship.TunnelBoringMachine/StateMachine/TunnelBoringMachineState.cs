using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public abstract class TunnelBoringMachineState
        {
            #region properties

            internal Program TunnelBoringMachineProgram { get; set; }

            #endregion

            #region construction

            internal TunnelBoringMachineState(Program tunnelBoringMachineProgram, Color statusColor)
            {
                TunnelBoringMachineProgram = tunnelBoringMachineProgram;

                foreach (var statusLight in TunnelBoringMachineProgram.StatusLights)
                {
                    statusLight.Color = statusColor;
                }
            }

            #endregion

            #region state machine

            public abstract void RunStateMachine();

            internal abstract TunnelBoringMachineStateDc GetState();

            #endregion
        }
    }
}
