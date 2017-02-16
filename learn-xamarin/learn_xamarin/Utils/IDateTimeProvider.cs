using System;

namespace learn_xamarin.Utils
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}