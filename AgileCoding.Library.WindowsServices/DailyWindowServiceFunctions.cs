using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AgileCoding.Library.WindowsServices
{
    public static class DailyWindowServiceFunctions
    {
        public static bool InitializeTimer(TimeSpan startTime, 
            TimeSpan stopTime, 
            ref Timer batchProcessTimer, 
            ElapsedEventHandler processBatch, 
            int batchPorcessTimeInterval, 
            bool alreadyScheduled = false)
        {
            if (!alreadyScheduled && CheckBatchStart(startTime, stopTime))
            {
                batchProcessTimer = new Timer(batchPorcessTimeInterval);
                alreadyScheduled = true;
            }
            else if (!CheckBatchStart(startTime, stopTime))
            {
                batchProcessTimer = new Timer(GetTimerDelayBeforeStartInMilliseconds(startTime, stopTime).Value.TotalMilliseconds);
                alreadyScheduled = false;
            }

            batchProcessTimer.Elapsed += processBatch;
            batchProcessTimer.Enabled = true;

            return alreadyScheduled;
        }

        public static bool CheckBatchStart(TimeSpan startTime, TimeSpan stopTime, TimeSpan timeNowOverride)
        {
            if (startTime < stopTime)
            {
                return timeNowOverride > startTime && (timeNowOverride < stopTime);
            }
            else if (timeNowOverride < new TimeSpan(23, 59, 59) && timeNowOverride > startTime)
            {
                return true;
            }
            else if (timeNowOverride >= new TimeSpan(00, 00, 00) && timeNowOverride < stopTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckBatchStart(TimeSpan startTime, TimeSpan stopTime)
        {
            return CheckBatchStart(startTime, stopTime, DateTime.Now.TimeOfDay);
        }
        
        public static TimeSpan? GetTimerDelayBeforeStartInMilliseconds(TimeSpan startTime, TimeSpan stopTime)
        {
            return GetTimerDelayBeforeStartInMilliseconds(startTime, stopTime, DateTime.Now.TimeOfDay);
        }

        public static TimeSpan? GetTimerDelayBeforeStartInMilliseconds(TimeSpan startTime, TimeSpan stopTime, TimeSpan timeNowOverride)
        {
            long timeDiffInMilliseconds = 0;
            if (startTime < stopTime)
            {
                if (timeNowOverride > startTime && timeNowOverride < stopTime)
                {
                    return null;
                }
                else if (timeNowOverride >= stopTime)
                {
                    timeDiffInMilliseconds = (new TimeSpan(23, 59, 59) - timeNowOverride).Ticks + startTime.Ticks;
                }
                else
                {
                    timeDiffInMilliseconds = (timeNowOverride - startTime).Ticks;
                }
            }
            else
            {
                if (timeNowOverride > startTime || timeNowOverride < stopTime)
                {
                    return null;
                }
                else if (timeNowOverride < startTime)
                {
                    timeDiffInMilliseconds = (startTime - timeNowOverride).Ticks;
                }
                else if (timeNowOverride > stopTime)
                {
                    timeDiffInMilliseconds = (timeNowOverride - startTime).Ticks;
                }
            }

            return new TimeSpan(timeDiffInMilliseconds);
        }
    }
}
