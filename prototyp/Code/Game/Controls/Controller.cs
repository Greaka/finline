using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Game.Helper;

namespace prototyp.Code.Game.Controls
{
    class Controller
    {
        public delegate void Shot();

        public event Shot Shoot;

        private bool shootable = true;
        private bool alreadyshot = false;
        const double trigger = 0.2;

        public void ControlsLoop()
        {
            var aTimer = new Timer
            {
                Interval = 1000/ControlsHelper.ActualShotsPerSecond,
                Enabled = true
            };
            aTimer.Elapsed += (sender, args) => { shootable = true; };
            
            while (ControlsHelper.Active)
            {
                if (Math.Abs(aTimer.Interval - ControlsHelper.ActualShotsPerSecond) < 0.00001)
                    aTimer.Interval = ControlsHelper.ActualShotsPerSecond;
                var inputstate = GamePad.GetState(0);
                if (inputstate.IsConnected)
                {
                    ControlsHelper.MoveDirection = inputstate.ThumbSticks.Left;
                    ControlsHelper.ShootDirection = inputstate.ThumbSticks.Right;
                    Shootroutine(inputstate.Triggers.Right > trigger);
                }
                else
                {
                    //TODO: ControlsHelper.MoveDirection = inputstate.ThumbSticks.Left;
                    //TODO: ControlsHelper.ShootDirection = inputstate.ThumbSticks.Right;
                    Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed);
                }
            }
        }

        private void Shootroutine(bool shootPressed)
        {
            Action beforeShoot = () =>
            {
                alreadyshot = true;
                shootable = false;
                Shoot();
            };

            if (!shootable) return;
            if (ControlsHelper.ActualShootMode == Enums.EWeaponShootMode.SingleFire)
            {
                if (alreadyshot)
                {
                    if (!shootPressed)
                        alreadyshot = false;
                }
                else
                    if (shootPressed)
                        beforeShoot();
            }
            else
                if (shootPressed)
                    beforeShoot();
        }
    }
}
