using System;
using Microsoft.Xna.Framework;
using prototyp.Code.Constants;
using prototyp.Code.Utility;

namespace prototyp.Code.Game.Helper
{
    public static class ControlsHelper
    {
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

        private static readonly ThreadSafeObject<double> sps = new ThreadSafeObject<double>(2.5);
        public static double ActualShotsPerSecond
        {
            get { return sps.value; }
            set
            {
                lock (sps)
                {
                    sps.value = value;
                }
            }
        }
        /*
        private static ThreadSafeObject<bool> forward = new ThreadSafeObject<bool>(false);
        public static bool Forward
        {
            get { return forward.value; }
            set
            {
                lock (forward)
                {
                    forward.value = value;
                }
            }
        }

        private static ThreadSafeObject<bool> backward = new ThreadSafeObject<bool>(false);
        public static bool Backward
        {
            get { return backward.value; }
            set
            {
                lock (backward)
                {
                    backward.value = value;
                }
            }
        }

        private static ThreadSafeObject<bool> right = new ThreadSafeObject<bool>(false);
        public static bool Right
        {
            get { return right.value; }
            set
            {
                lock (right)
                {
                    right.value = value;
                }
            }
        }

        private static ThreadSafeObject<bool> left = new ThreadSafeObject<bool>(false);
        public static bool Left
        {
            get { return left.value; }
            set
            {
                lock (left)
                {
                    left.value = value;
                }
            }
        }*/

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

        private static readonly ThreadSafeObject<Vector2> moveDirection = new ThreadSafeObject<Vector2>(new Vector2(0));
        public static Vector2 MoveDirection
        {
            get
            {
                if (!(moveDirection.value.Length() > 0)) return moveDirection.value;
                var perspective = GraphicConstants.CameraOffset.get2d();
                perspective.Normalize();
                perspective = moveDirection.value.rotate((float)Math.PI + perspective.getAngle());
                perspective.Normalize();
                return perspective;
            }
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
            get
            {
                if (!(shootDirection.value.Length() > 0)) return shootDirection.value;
                var perspective = GraphicConstants.CameraOffset.get2d();
                perspective.Normalize();
                perspective = shootDirection.value.rotate((float)Math.PI + perspective.getAngle());
                perspective.Normalize();
                return perspective;
            }
            set
            {
                lock (shootDirection)
                {
                    shootDirection.value = value;
                }
            }
        }

        private static readonly ThreadSafeObject<GameConstants.EWeaponShootMode> actualShootMode = new ThreadSafeObject<GameConstants.EWeaponShootMode>(GameConstants.EWeaponShootMode.SingleFire);
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