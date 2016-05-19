using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototyp.Code.Game.Helper
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
