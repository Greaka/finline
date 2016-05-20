using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using prototyp.Code.Constants;
using prototyp.Code.Game.Helper;
using prototyp.Code.Utility;
using Timer = System.Timers.Timer;

namespace prototyp.Code.Game.Controls
{
    class Controller
    {
        public delegate void Shot();

        public event Shot Shoot;

        private readonly Timer aTimer;
        private bool shootable = true;
        private bool alreadyshot = false;
        const double trigger = 0.2;

        public Controller()
        {
            aTimer = new Timer
            {
                Interval = 1000/ControlsHelper.ActualShotsPerSecond,
                Enabled = true
            };
            aTimer.Elapsed += (sender, args) => { shootable = true; };
        }

        public void Update() {
            if (Math.Abs(aTimer.Interval - ControlsHelper.ActualShotsPerSecond) < 0.00001)
                aTimer.Interval = ControlsHelper.ActualShotsPerSecond;
            var inputstate = GamePad.GetState(PlayerIndex.One);
            if (inputstate.IsConnected)
            {
                ControlsHelper.MoveDirection = inputstate.ThumbSticks.Left;
                ControlsHelper.ShootDirection = inputstate.ThumbSticks.Right;
                Shootroutine(inputstate.Triggers.Right > trigger);
            }
            else
            {
                var moveDirection = new Vector2(0);
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    moveDirection += Vector2.UnitY;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    moveDirection -= Vector2.UnitY;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    moveDirection -= Vector2.UnitX;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    moveDirection += Vector2.UnitX;

                ControlsHelper.MoveDirection = moveDirection;

                ControlsHelper.ShootDirection = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(ControlsHelper.ViewMatrix)) -
                    new Vector2(ControlsHelper.PlayerPosition.X, ControlsHelper.PlayerPosition.Y);
                ControlsHelper.ShootDirection.Normalize();
                Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
        }

        private void Shootroutine(bool shootPressed)
        {
            Action beforeShoot = () =>
            {
                if (!shootPressed) return;
                alreadyshot = true;
                shootable = false;
                Shoot?.Invoke();
            };

            if (!shootable) return;
            if (ControlsHelper.ActualShootMode == GameConstants.EWeaponShootMode.SingleFire)
            {
                if (alreadyshot)
                {
                    if (!shootPressed)
                        alreadyshot = false;
                }
                else
                    beforeShoot();
            }
            else
                beforeShoot();
        }
    }
}
