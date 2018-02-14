using System;
using System.Collections.Generic;

namespace learn_xamarin.AppSettings
{
    public interface ISettingsRepo
    {
        string MainCurrency { get; set; }
        string CurrentCurrency { get; set; }
        IObservable<KeyValuePair<string, string>> SettingsUpdated { get; }
    }
}