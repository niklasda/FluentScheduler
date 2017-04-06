using System;

namespace FluentScheduler
{
    /// <summary>
    /// Unit of time that represents a specific day of the month.
    /// </summary>
    public sealed class MonthOnWeekDayOfMonthUnit
    {
        private readonly int _duration;

        private readonly int _weekDayOfMonth;

        internal MonthOnWeekDayOfMonthUnit(Schedule schedule, int duration, int weekDayOfMonth)
        {
            _duration = duration;
            _weekDayOfMonth = weekDayOfMonth;
            Schedule = schedule;
            At(0, 0);
        }

        internal Schedule Schedule { get; private set; }

        /// <summary>
        /// Runs the job at the given time of day.
        /// </summary>
        /// <param name="hours">The hours (0 through 23).</param>
        /// <param name="minutes">The minutes (0 through 59).</param>
        public void At(int hours, int minutes)
        {
            Schedule.CalculateNextRun = x =>
            {
                DateTime nextRun;
                if (x.Date.First().IsWeekday())
                {
                    nextRun = x.Date.First().NextNWeekday(_weekDayOfMonth - 1).AddHours(hours).AddMinutes(minutes);
                }
                else
                {
                    nextRun = x.Date.First().NextNWeekday(_weekDayOfMonth ).AddHours(hours).AddMinutes(minutes);
                }

                if (x > nextRun)
                {
                    if (x.Date.First().AddMonths(_duration).IsWeekday())
                    {
                        nextRun = x.Date.First().AddMonths(_duration).NextNWeekday(_weekDayOfMonth - 1).AddHours(hours).AddMinutes(minutes);
                    }
                    else
                    {
                        nextRun = x.Date.First().AddMonths(_duration).NextNWeekday(_weekDayOfMonth).AddHours(hours).AddMinutes(minutes);
                    }
                    return nextRun;
                }

                return nextRun;
            };
        }
    }
}
