using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AgileCoding.Library.WindowsServices
{
    public static class ContinuousIntervalServiceFunctions
    {
        public static void InitializeIntervalTimer(ref Timer fileWatcherTimer, ElapsedEventHandler hanlder, int timerInterval)
        {
            fileWatcherTimer = new Timer
            {
                Interval = timerInterval
            };

            fileWatcherTimer.Elapsed += hanlder;
            fileWatcherTimer.Enabled = true;
        }

        public static void ResetTimer(ref Timer fileWatcherTimer, int timerInterval)
        {
            if (fileWatcherTimer != null)
            {
                fileWatcherTimer.Interval = timerInterval;
                fileWatcherTimer.Enabled = true;
            }
        }
    }
}
