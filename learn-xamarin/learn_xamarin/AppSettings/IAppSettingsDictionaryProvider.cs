using System.Collections.Generic;

namespace learn_xamarin.AppSettings
{
    public interface IAppSettingsDictionaryProvider
    {
        IDictionary<string, object> Settings { get; }
    }
}