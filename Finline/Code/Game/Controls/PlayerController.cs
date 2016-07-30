using System;

using Finline.Code.Constants;
using Finline.Code.Utility;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Timer = System.Timers.Timer;

namespace Finline.Code.Game.Controls
{
    public class PlayerController
    {
        public delegate void Shot(Vector3 position, Vector2 direction);
        public event Shot Shoot;

        private readonly Timer aTimer;
        private bool shootable = true;
        private bool alreadyShot = false;

        private GameConstants.EWeaponShootMode actualShootMode = GameConstants.EWeaponShootMode.Automatic;

        private const float shotsPerSecond = 4;

        private const double trigger = 0.2;

        public PlayerController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000/shotsPerSecond, 
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update(GraphicsDevice device, 
            out Vector2 moveDirection, 
            ref Vector2 shootDirection, 
            Vector3 playerPosition, 
            Matrix projectionMatrix, 
            Matrix viewMatrix)
        {
            if (Math.Abs(this.aTimer.Interval - shotsPerSecond) < 0.00001) this.aTimer.Interval = shotsPerSecond;
            var inputstate = GamePad.GetState(PlayerIndex.One);
            if (inputstate.IsConnected)
            {
                moveDirection = inputstate.ThumbSticks.Left.addPerspective();
                if (inputstate.ThumbSticks.Right.Length() > 0)
                {
                    shootDirection = inputstate.ThumbSticks.Right.addPerspective();
                }
                this.Shootroutine(inputstate.Triggers.Right > trigger, shootDirection, playerPosition);
            }
            else
            {
                var moveDir = new Vector2(0);
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    moveDir += Vector2.UnitY;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    moveDir -= Vector2.UnitY;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    moveDir -= Vector2.UnitX;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    moveDir += Vector2.UnitX;

                moveDirection = moveDir.addPerspective();
                
                var shootDir = MousePosition(device, projectionMatrix, viewMatrix).get2d()
                    - playerPosition.get2d();
                if (shootDir.Length() > 0)
                {
                    shootDir.Normalize();
                    shootDirection = shootDir;
                }
                this.Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed, shootDirection, playerPosition);
            }
        }

        private static Vector3 MousePosition(GraphicsDevice device, Matrix projectionMatrix, Matrix viewMatrix)
        {
            var plane = new Plane(Vector3.Zero, Vector3.UnitX, Vector3.UnitY);
            var ms = Mouse.GetState().Position.ToVector2();
            var nearScreenPoint = new Vector3(ms, 0);
            var farScreenPoint = new Vector3(ms, 1);
            var nearWorldPoint = device.Viewport.Unproject(
                nearScreenPoint, 
                projectionMatrix, 
                viewMatrix, 
                Matrix.Identity);
            var farWorldPoint = device.Viewport.Unproject(
                farScreenPoint, 
                projectionMatrix, 
                viewMatrix, 
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

        private void Shootroutine(bool shootPressed, Vector2 shootDirection, Vector3 playerPosition)
        {
            Action beforeShoot = () =>
            {
                if (!shootPressed) return;
                this.alreadyShot = true;
                this.shootable = false;
                this.Shoot?.Invoke(playerPosition, shootDirection);
            };

            if (!this.shootable) return;
            if (this.actualShootMode == GameConstants.EWeaponShootMode.SingleFire)
            {
                if (this.alreadyShot)
                {
                    if (!shootPressed) this.alreadyShot = false;
                }
                else
                    beforeShoot();
            }
            else
                beforeShoot();
        }
    }
}
