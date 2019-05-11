using System;

namespace CSPspEmu.Utils
{
    static public class TimeExt
    {
        static public TimeSpan Milliseconds(this int value) => TimeSpan.FromMilliseconds(value);
        static public TimeSpan Seconds(this int value) => TimeSpan.FromSeconds(value);

        static public TimeSpan Milliseconds(this double value) => TimeSpan.FromMilliseconds(value);
        static public TimeSpan Seconds(this double value) => TimeSpan.FromSeconds(value);
    }
}