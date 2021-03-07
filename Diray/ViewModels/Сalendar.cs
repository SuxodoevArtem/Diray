using System;
using System.Collections.Generic;
using System.Text;

namespace Diray.ViewModels
{
    class Сalendar
    {
        private DateTime _currentDate;
        public List<DateTime> Days = new List<DateTime>();
        private DateTime startDate;
      
        public Сalendar(DateTime currentDate)
        {
            Days.Clear();
            this._currentDate = currentDate;
            startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        }
        public void AddMonth(DateTime currentDay)
        {
            this.startDate = currentDay;
        }

        public List<DateTime> GetDays()
        {
            if(startDate.DayOfWeek != DayOfWeek.Monday)
            {
                for(;;)
                {
                    startDate = startDate.AddDays(-1);
                    if (startDate.DayOfWeek == DayOfWeek.Monday) break;
                }
            }

            for(int i = 0; i < 35; i++)
            {
                Days.Add(startDate);
                startDate = startDate.AddDays(1);
            }

            return Days;
        }
    }
}
