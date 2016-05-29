namespace Finline.Code.Game.Helper
{
    public class ThreadSafeObject<T>
    {
        public T value;

        public ThreadSafeObject(T data)
        {
            this.value = data;
        }
    }
}
