using System;

namespace learn_xamarin.Model
{
    public class UnsynchronizedItem // todo piotr still needed?
    {
        public Guid Id { get; set; }
    }
    
    public class ConfigEntry
    {
        public string Key { get; set; }
        public string Value { get; set; }
        
    }
}
