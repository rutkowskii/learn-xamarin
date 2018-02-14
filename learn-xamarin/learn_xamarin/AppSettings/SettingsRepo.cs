using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using learn_xamarin.Model;

namespace learn_xamarin.AppSettings
{
    public class SettingsRepo : ISettingsRepo
    {
        private readonly object _monitor;
        private readonly Subject<KeyValuePair<string, string>> _settingsUpdatesSubject;
        private readonly IAppSettingsDictionaryProvider _settingsDictionaryProvider;

        public SettingsRepo(IAppSettingsDictionaryProvider settingsDictionaryProvider)
        {
            _settingsDictionaryProvider = settingsDictionaryProvider;
            _monitor = new object();
            _settingsUpdatesSubject = new Subject<KeyValuePair<string, string>>();
            InitializeWithDefaultsIfNeeded();
        }

        public string MainCurrency
        {
            get => Get(SettingsKeys.MainCurrency);
            set => Set(SettingsKeys.MainCurrency, value);
        }

        public string CurrentCurrency
        {
            get => Get(SettingsKeys.CurrentCurrency);
            set => Set(SettingsKeys.CurrentCurrency, value);
        }

        private string Get(string key)
        {
            return (string)SettingsDictionary.GetOrDefault(key);
        }

        private void Set(string key, string value)
        {
            lock(_monitor)
            {
                SettingsDictionary[key] = value;
                _settingsUpdatesSubject.OnNext(new KeyValuePair<string, string>(key, value));
            }
        }

        public IObservable<KeyValuePair<string, string>> SettingsUpdated => _settingsUpdatesSubject.AsObservable();

        private void InitializeWithDefaultsIfNeeded()
        {
            lock (_monitor)
            {
                if (MainCurrency == null)
                {
                    MainCurrency = Currency.Pln.Code;
                }
                if (CurrentCurrency == null)
                {
                    CurrentCurrency = Currency.Gbp.Code;
                }
            }
        }

        private IDictionary<string, object> SettingsDictionary => _settingsDictionaryProvider.Settings;
    }
}