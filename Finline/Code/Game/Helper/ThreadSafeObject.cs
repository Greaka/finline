﻿namespace prototyp.Code.Game.Helper
{
    public class ThreadSafeObject<T>
    {
        public T value;

        public ThreadSafeObject(T data)
        {
            value = data;
        }
    }
}