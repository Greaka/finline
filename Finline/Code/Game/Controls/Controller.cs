using System;

using Finline.Code.Constants;
using Finline.Code.Game.Helper;
using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Timer = System.Timers.Timer;

namespace Finline.Code.Game.Controls
{
    public class Controller
    {
        public delegate void Shot();
        public event Shot Shoot;

        private readonly Timer aTimer;
        private bool shootable = true;
        private bool alreadyshot = false;

        private const double trigger = 0.2;

        public Controller()
        {
            this.aTimer = new Timer
            {
                Interval = 1000/ControlsHelper.ActualShotsPerSecond, 
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update(GraphicsDevice device) {
            if (Math.Abs(this.aTimer.Interval - ControlsHelper.ActualShotsPerSecond) < 0.00001) this.aTimer.Interval = ControlsHelper.ActualShotsPerSecond;
            var inputstate = GamePad.GetState(PlayerIndex.One);
            if (inputstate.IsConnected)
            {
                ControlsHelper.MoveDirection = inputstate.ThumbSticks.Left;
                ControlsHelper.ShootDirection = inputstate.ThumbSticks.Right.addPerspective();
                this.Shootroutine(inputstate.Triggers.Right > trigger);
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
                
                var shootDirection = MousePosition(device).get2d()
                    - ControlsHelper.PlayerPosition.get2d();
                shootDirection.Normalize();
                ControlsHelper.ShootDirection = shootDirection;
                this.Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed);
            }
        }

        private static Vector3 MousePosition(GraphicsDevice device)
        {
            var plane = new Plane(Vector3.Zero, Vector3.UnitX, Vector3.UnitY);
            var ms = Mouse.GetState().Position.ToVector2();
            var nearScreenPoint = new Vector3(ms, 0);
            var farScreenPoint = new Vector3(ms, 1);
            var nearWorldPoint = device.Viewport.Unproject(
                nearScreenPoint, 
                ControlsHelper.ProjectionMatrix, 
                ControlsHelper.ViewMatrix, 
                Matrix.Identity);
            var farWorldPoint = device.Viewport.Unproject(
                farScreenPoint, 
                ControlsHelper.ProjectionMatrix, 
                ControlsHelper.ViewMatrix, 
                Matrix.Identity);

            var direction = farWorldPoint - nearWorldPoint;
            var angle = Vector3.Dot(plane.Normal, direction);

            // Part 1: Find if the ray is parallel to the plane by checking 
            // if the angle between them is zero. 
            if (!(Math.Abs(angle) > float.Epsilon)) return Vector3.Zero;
            var v1D = Vector3.Dot(plane.Normal, nearWorldPoint);

            // Part 2: Extend the starting point to the distance it would 
            // require to intersect the plane.
            return nearWorldPoint + direction * ((plane.D - v1D) / angle);
        }

        private void Shootroutine(bool shootPressed)
        {
            Action beforeShoot = () =>
            {
                if (!shootPressed) return;
                this.alreadyshot = true;
                this.shootable = false;
                this.Shoot?.Invoke();
            };

            if (!this.shootable) return;
            if (ControlsHelper.ActualShootMode == GameConstants.EWeaponShootMode.SingleFire)
            {
                if (this.alreadyshot)
                {
                    if (!shootPressed) this.alreadyshot = false;
                }
                else
                    beforeShoot();
            }
            else
                beforeShoot();
        }
    }
}
