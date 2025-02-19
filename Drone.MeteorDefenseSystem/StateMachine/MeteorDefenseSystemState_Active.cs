using VRageMath;
using VRage.Game.ModAPI.Ingame;
using System.Linq;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        public class MeteorDefenseSystemState_Active : MeteorDefenseSystemState
        {
            #region construction

            internal MeteorDefenseSystemState_Active(Program meteorDefenseSystemProgram)
                : base(meteorDefenseSystemProgram, Color.Green)
            {
                meteorDefenseSystemProgram.Turrets.ForEach(t => t.Enabled = true);
            }

            #endregion

            #region state machine

            public override void RunStateMachine()
            {
                // if power is less than 10% then fallback to station to reload
                var currentStoredPower = 0.0;
                MeteorDefenseSystemProgram.BatteryBlocks.ForEach(b => currentStoredPower += b.CurrentStoredPower);

                if (currentStoredPower / MeteorDefenseSystemProgram.MaxStoredPower <= 0.1)
                {
                    MeteorDefenseSystemProgram.ChangeState(new MeteorDefenseSystemState_Fallback(MeteorDefenseSystemProgram));
                }

                // if ammo is low and turret has no target then fallback to station
                if (!MeteorDefenseSystemProgram.Turrets.Any(t => t.HasTarget)
                    & !MeteorDefenseSystemProgram.Turrets
                    .TrueForAll(t => t.GetInventory().ContainItems(4, MyItemType.MakeAmmo("NATO_25x184mm"))))
                {
                    MeteorDefenseSystemProgram.ChangeState(new MeteorDefenseSystemState_Fallback(MeteorDefenseSystemProgram));
                }

                // if ammo is empty then fallback to station
                if (!MeteorDefenseSystemProgram.Turrets
                    .TrueForAll(t => t.GetInventory().ContainItems(0, MyItemType.MakeAmmo("NATO_25x184mm"))))
                {
                    MeteorDefenseSystemProgram.ChangeState(new MeteorDefenseSystemState_Fallback(MeteorDefenseSystemProgram));
                }

                if (!MeteorDefenseSystemProgram.ReferenceControl.IsAutoPilotEnabled)
                {
                    MeteorDefenseSystemProgram.MoveToRelativePosition(MeteorDefenseSystemProgram._angle.ToString(), MeteorDefenseSystemProgram.CalculateCircleCoordinates(MeteorDefenseSystemProgram._angle));
                    MeteorDefenseSystemProgram.ReferenceControl.SetAutoPilotEnabled(true);
                    MeteorDefenseSystemProgram._angle += 30;
                    MeteorDefenseSystemProgram._angle %= 360;
                }
            }

            internal override MeteorDefenseSystemStateDc GetState()
            {
                return MeteorDefenseSystemStateDc.Active;
            }

            #endregion
        }
    }
}
