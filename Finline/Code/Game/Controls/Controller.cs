using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public void Update(GraphicsDevice device) {
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

                var shootDirection3d = MousePosition(device);
                var shootDirection = new Vector2(shootDirection3d.X, shootDirection3d.Z);
                    //- ControlsHelper.PlayerPosition.get2d();
                shootDirection.Normalize();
                ControlsHelper.ShootDirection = shootDirection;
                Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
        }

        private static Vector3 MousePosition(GraphicsDevice device)
        {
            var ms = Mouse.GetState();
            var nearScreenPoint = new Vector3(ms.Position.ToVector2(), 0);
            var farScreenPoint = new Vector3(ms.Position.ToVector2(), 1);
            var nearWorldPoint = device.Viewport.Unproject(nearScreenPoint, ControlsHelper.ProjectionMatrix, ControlsHelper.ViewMatrix, Matrix.Identity);
            var farWorldPoint = device.Viewport.Unproject(farScreenPoint, ControlsHelper.ProjectionMatrix, ControlsHelper.ViewMatrix, Matrix.Identity);

            var direction = farWorldPoint - nearWorldPoint;
            var zFactor = -nearWorldPoint.Y / direction.Y;
            var zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
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
