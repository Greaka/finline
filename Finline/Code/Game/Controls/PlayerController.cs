﻿namespace Finline.Code.Game.Controls
{
    using System;
    using System.Timers;

    using Finline.Code.Constants;
    using Finline.Code.Game.Entities;
    using Finline.Code.Game.Entities.LivingEntity;
    using Finline.Code.Utility;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PlayerController
    {
        public delegate void Shot(Entity firedFrom, Vector2 direction, int index);
        public event Shot Shoot;

        private readonly Timer aTimer;
        private bool shootable = true;
        private bool alreadyShot;

        private readonly GameConstants.EWeaponShootMode actualShootMode = GameConstants.EWeaponShootMode.Automatic;

        private const float ShotsPerSecond = 4;

        private const double Trigger = 0.2;

        public PlayerController()
        {
            this.aTimer = new Timer
            {
                Interval = 1000/ShotsPerSecond, 
                Enabled = true
            };
            this.aTimer.Elapsed += (sender, args) => { this.shootable = true; };
        }

        public void Update(GraphicsDevice device, 
            out Vector2 moveDirection, 
            ref Vector2 shootDirection, 
            Player player, 
            Matrix projectionMatrix, 
            Matrix viewMatrix)
        {
            if (player.Dead) this.shootable = false;
            if (Math.Abs(this.aTimer.Interval - ShotsPerSecond) < 0.00001) this.aTimer.Interval = ShotsPerSecond;
            var inputstate = GamePad.GetState(PlayerIndex.One);
            if (inputstate.IsConnected)
            {
                moveDirection = inputstate.ThumbSticks.Left.AddPerspective();
                if (inputstate.ThumbSticks.Right.Length() > 0)
                {
                    shootDirection = inputstate.ThumbSticks.Right.AddPerspective();
                }

                this.Shootroutine(inputstate.Triggers.Right > Trigger, shootDirection, player, 0);
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

                moveDirection = moveDir.AddPerspective();
                
                var shootDir = MousePosition(device, projectionMatrix, viewMatrix).Get2D()
                    - player.Position.Get2D();
                if (shootDir.Length() > 0)
                {
                    shootDir.Normalize();
                    shootDirection = shootDir;
                }

                this.Shootroutine(Mouse.GetState().LeftButton == ButtonState.Pressed, shootDirection, player, 0);
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

        private void Shootroutine(bool shootPressed, Vector2 shootDirection, Player player, int index)
        {
            Action beforeShoot = () =>
            {
                if (!shootPressed) return;
                this.alreadyShot = true;
                this.shootable = false;
                this.Shoot?.Invoke(player, shootDirection, index);
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
