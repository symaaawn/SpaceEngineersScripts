using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_Error : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_Error(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.Red)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.DrillsOff();
                TunnelBoringMachineProgram.WeldersOff();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.Error;
            }

            #endregion
        }
    }
}
