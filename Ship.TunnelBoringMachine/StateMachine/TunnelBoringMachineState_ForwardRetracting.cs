using Sandbox.ModAPI.Ingame;
using System.Linq;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class TunnelBoringMachineState_ForwardRetracting : TunnelBoringMachineState
        {
            #region construction

            public TunnelBoringMachineState_ForwardRetracting(Program tunnelBoringMachineProgram) : base(tunnelBoringMachineProgram, Color.Green)
            {
                TunnelBoringMachineProgram.StartRotor();
                TunnelBoringMachineProgram.MoveBackward();
                TunnelBoringMachineProgram.DrillsOn();
                TunnelBoringMachineProgram.ProjectorsOff();
                TunnelBoringMachineProgram.WeldersOff();
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

                    if (TunnelBoringMachineProgram.PistonsFront.All(piston => piston.CurrentPosition == piston.MinLimit) && TunnelBoringMachineProgram.PistonsBack.All(piston => piston.CurrentPosition == piston.MaxLimit))
                    {
                        TunnelBoringMachineProgram._state = new TunnelBoringMachineState_ForwardBackMerging(TunnelBoringMachineProgram);
                    }
                }
            }

            internal override TunnelBoringMachineStateDc GetState()
            {
                return TunnelBoringMachineStateDc.ForwardRetracting;
            }

            #endregion
        }
    }
}