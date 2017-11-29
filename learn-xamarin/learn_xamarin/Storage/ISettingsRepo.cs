namespace learn_xamarin.Storage
{
    public interface ISettingsRepo
    {
        void Set(string key, string value);
        string Get(string key);
    }
}