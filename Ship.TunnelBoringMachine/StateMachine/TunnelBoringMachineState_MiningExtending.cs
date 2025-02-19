using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_MiningExtending : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_MiningExtending(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.DarkOliveGreen)
            {
                TunnelBoringMachineProgram.StopPistons();
                TunnelBoringMachineProgram.EngageBorerPistons();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram.PistonsBorer.All(piston => piston.CurrentPosition == piston.MaxLimit))
                {
                    TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_MiningRetracting(TunnelBoringMachineProgram));
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.MiningExtending;
            }

            #endregion
        }
    }
}