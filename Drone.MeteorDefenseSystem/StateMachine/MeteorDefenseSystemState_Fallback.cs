using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class MeteorDefenseSystemState_Fallback : MeteorDefenseSystemState
        {
            #region construction

            internal MeteorDefenseSystemState_Fallback(Program meteorDefenseSystemProgram)
                : base(meteorDefenseSystemProgram, Color.OrangeRed)
            {
                meteorDefenseSystemProgram.Turrets.ForEach(t => t.Enabled = false);
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                if (!MeteorDefenseSystemProgram.ReferenceControl.IsAutoPilotEnabled)
                {
                    MeteorDefenseSystemProgram.MoveToRelativePosition(MeteorDefenseSystemProgram._angle.ToString(), MeteorDefenseSystemProgram.CalculateCircleCoordinates(MeteorDefenseSystemProgram._angle));
                    MeteorDefenseSystemProgram.ReferenceControl.SetAutoPilotEnabled(true);
                    if (MeteorDefenseSystemProgram._angle != 0)
                    {
                        MeteorDefenseSystemProgram._angle += 30;
                        MeteorDefenseSystemProgram._angle %= 360;
                    }
                }
            }

            internal override MeteorDefenseSystemStateDc GetState()
            {
                return MeteorDefenseSystemStateDc.Fallback;
            }

            #endregion

        }
    }
}
