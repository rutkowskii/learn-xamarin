using learn_xamarin.Model;
using Xamarin.Forms;

namespace learn_xamarin.Storage
{
    public class SettingsRepo : ISettingsRepo
    {
        private readonly object _monitor;

        public SettingsRepo()
        {
            _monitor = new object();
            InitializeWithDefaultsIfNeeded();
        }
        
        public void Set(string key, string value)
        {
            lock(_monitor)
            {
                Application.Current.Properties[key] = value;
            }
        }

        public string Get(string key) // todo piotr this lock on get is not a good idea :/
        {
            lock (_monitor)
            {
                var propertiesDictionary = Application.Current.Properties;
                return propertiesDictionary.ContainsKey(key) ? propertiesDictionary[key] as string : null;
            }
        }

        private void InitializeWithDefaultsIfNeeded()
        {
            lock (_monitor)
            {
                if (Get(SettingsKeys.MainCurrency) == null)
                {
                    Set(SettingsKeys.MainCurrency, Currency.Pln.Code);
                }
                if (Get(SettingsKeys.CurrentCurrency) == null)
                {
                    Set(SettingsKeys.CurrentCurrency, Currency.Gbp.Code);
                }
            }
        }
    }
}