namespace SimpleLruCache
{
    interface ICache
    {
        void Clear();

        object Get(object key);

        void Remove(object key);

        void Set(object key, object value);
    }
}
