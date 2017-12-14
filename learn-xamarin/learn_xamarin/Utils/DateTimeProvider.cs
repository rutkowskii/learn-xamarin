using System;

namespace learn_xamarin.Utils
{
    class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}