using System.Collections.Generic;
using Xamarin.Forms;

namespace learn_xamarin.AppSettings
{
    class AppSettingsDictionaryProvider : IAppSettingsDictionaryProvider
    {
        public IDictionary<string, object> Settings => Application.Current.Properties;
    }
}