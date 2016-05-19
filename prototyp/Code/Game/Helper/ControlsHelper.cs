using Microsoft.Xna.Framework;

namespace prototyp.Code.Game.Helper
{
    public static class ControlsHelper
    {
        private static ThreadSafeObject<bool> active = new ThreadSafeObject<bool>(true);
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


        private static ThreadSafeObject<double> sps = new ThreadSafeObject<double>(2.5);
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
        }

        private static ThreadSafeObject<Vector2> moveDirection = new ThreadSafeObject<Vector2>(new Vector2());
        public static Vector2 MoveDirection
        {
            get { return moveDirection.value; }
            set
            {
                lock (moveDirection)
                {
                    moveDirection.value = value;
                }
            }
        }

        private static ThreadSafeObject<Vector2> shootDirection = new ThreadSafeObject<Vector2>(new Vector2());
        public static Vector2 ShootDirection
        {
            get { return shootDirection.value; }
            set
            {
                lock (shootDirection)
                {
                    shootDirection.value = value;
                }
            }
        }

        private static ThreadSafeObject<Enums.EWeaponShootMode> actualShootMode = new ThreadSafeObject<Enums.EWeaponShootMode>(Enums.EWeaponShootMode.SingleFire);
        public static Enums.EWeaponShootMode ActualShootMode
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

        private static ThreadSafeObject<Enums.EWeaponType> actualWeaponMode = new ThreadSafeObject<Enums.EWeaponType>(Enums.EWeaponType.Pistol);
        public static Enums.EWeaponType ActualWeaponMode
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