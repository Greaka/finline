using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Finline.Code.Constants;
using Finline.Code.Game.Entities;
using Microsoft.Xna.Framework;
using Finline.Code.Utility;

namespace Finline.Code.Game.Helper
{
    public static class ControlsHelper
    {
        public static readonly ConcurrentDictionary<int, EnvironmentObject> EnvironmentObjects = new ConcurrentDictionary<int, EnvironmentObject>();
        public static readonly ConcurrentDictionary<int, Projectile> Projectiles = new ConcurrentDictionary<int, Projectile>();

        private static readonly ThreadSafeObject<bool> active = new ThreadSafeObject<bool>(true);
        public static bool Active
        {
            get { return active.value; }
            set
            {
                lock (active)
                {
                    active.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<Matrix> viewMatrix = new ThreadSafeObject<Matrix>(new Matrix());
        public static Matrix ViewMatrix
        {
            get { return viewMatrix.value; }
            set
            {
                lock (viewMatrix)
                    viewMatrix.value = value;
            }
        }

        private static readonly ThreadSafeObject<Matrix> projectionMatrix = new ThreadSafeObject<Matrix>(new Matrix());
        public static Matrix ProjectionMatrix
        {
            get { return projectionMatrix.value; }
            set
            {
                lock (projectionMatrix)
                    projectionMatrix.value = value;
            }
        }

        private static readonly ThreadSafeObject<double> shotsPerSecond = new ThreadSafeObject<double>(20);
        public static double ActualShotsPerSecond
        {
            get { return shotsPerSecond.value; }
            set
            {
                lock (shotsPerSecond)
                {
                    shotsPerSecond.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<Vector3> playerPosition = new ThreadSafeObject<Vector3>(new Vector3(0));
        public static Vector3 PlayerPosition
        {
            get { return playerPosition.value; }
            set
            {
                lock (moveDirection)
                {
                    playerPosition.value = value;
                }
            }
        }

        private static Vector2 addPerspective(this Vector2 me)
        {
            if (!(me.Length() > 0)) return me;
            var perspective = GraphicConstants.CameraOffset.get2d();
            perspective.Normalize();
            perspective = me.rotate((float)Math.PI + perspective.getAngle());
            perspective.Normalize();
            return perspective;
        }

        private static readonly ThreadSafeObject<Vector2> moveDirection = new ThreadSafeObject<Vector2>(new Vector2(0));
        public static Vector2 MoveDirection
        {
            get { return moveDirection.value.addPerspective(); }
            set
            {
                lock (moveDirection)
                {
                    moveDirection.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<Vector2> shootDirection = new ThreadSafeObject<Vector2>(new Vector2(0, 1));
        public static Vector2 ShootDirection
        {
            get { return shootDirection.value.addPerspective(); }
            set
            {
                lock (shootDirection)
                {
                    shootDirection.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<GameConstants.EWeaponShootMode> actualShootMode = new ThreadSafeObject<GameConstants.EWeaponShootMode>(GameConstants.EWeaponShootMode.Automatic);
        public static GameConstants.EWeaponShootMode ActualShootMode
        {
            get { return actualShootMode.value; }
            set
            {
                lock (actualShootMode)
                {
                    actualShootMode.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<GameConstants.EWeaponType> actualWeaponMode = new ThreadSafeObject<GameConstants.EWeaponType>(GameConstants.EWeaponType.Pistol);
        public static GameConstants.EWeaponType ActualWeaponMode
        {
            get { return actualWeaponMode.value; }
            set
            {
                lock (actualWeaponMode)
                {
                    actualWeaponMode.value = value;
                }
            }
        }
    }
}