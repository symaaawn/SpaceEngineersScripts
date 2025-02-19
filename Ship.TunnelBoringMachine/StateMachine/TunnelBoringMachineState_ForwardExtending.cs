using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_ForwardExtending : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_ForwardExtending(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.Green)
            {
                TunnelBoringMachineProgram.StartRotor();
                TunnelBoringMachineProgram.MoveForward();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOn();
                TunnelBoringMachineProgram.WeldersOn();
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (TunnelBoringMachineProgram._mode == TunnelBoringMachineModeDc.Forward)
                {
                    if (TunnelBoringMachineProgram.PeripherialSensor.IsActive)
                    {
                        TunnelBoringMachineProgram.ChangeState(new TunnelBoringMachineState_MiningExtending(TunnelBoringMachineProgram));
                    }

                    if (TunnelBoringMachineProgram.PistonsFront.All(piston => piston.CurrentPosition == piston.MaxLimit) && TunnelBoringMachineProgram.PistonsBack.All(piston => piston.CurrentPosition == piston.MinLimit))
                    {
                        TunnelBoringMachineProgram._state = new TunnelBoringMachineState_ForwardFrontMerging(TunnelBoringMachineProgram);
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.ForwardExtending;
            }

            #endregion
        }
    }
}
