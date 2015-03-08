using System;

namespace Streamup.Events
{
    public static class DateHelper
    {
        public static DateTime SinceEpoch(this long milliseconds)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(milliseconds);
        }
    }
}