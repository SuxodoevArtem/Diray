using System;
using System.Collections.Generic;
using System.Text;

namespace Diray.ViewModels
{
    class Сalendar
    {
        private DateTime _currentDate;
        public List<DateTime> Day;
        private DateTime startDate;

        public Сalendar(DateTime currentDate)
        {
            this._currentDate = currentDate;
            startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        }
        public List<DateTime> GetDays()
        {

            return Day;
        }
    }
}
