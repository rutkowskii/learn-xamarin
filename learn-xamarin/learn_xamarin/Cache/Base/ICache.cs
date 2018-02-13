namespace learn_xamarin.Cache.Base
{
    public interface ICache<T> : IReadOnlyCache<T>
    {
        void Add(T items);
    }
}