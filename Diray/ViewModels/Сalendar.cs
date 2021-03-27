using System;
using System.Collections.Generic;
using System.Text;

namespace Diray.ViewModels
{
    class Сalendar
    {
        public DateTime _currentDate;
        public List<DateTime> Days = new List<DateTime>();
        private DateTime _startDate;

        public string currentMonth {
            get { 
                if(_startDate.Month < 10)
                {
                    return  "0" + (_startDate.Month - 1).ToString();
                }
                return _startDate.Month.ToString();
            }
        }

        public Сalendar(DateTime currentDate)
        {
            Days.Clear();
            this._currentDate = currentDate;
            _startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        }
        public void AddMonth(DateTime currentDay)
        {
            this._startDate = currentDay;
        }

        public List<DateTime> GetDays()
        {
            if(_startDate.DayOfWeek != DayOfWeek.Monday)
            {
                for(;;)
                {
                    _startDate = _startDate.AddDays(-1);
                    if (_startDate.DayOfWeek == DayOfWeek.Monday) break;
                }
            }

            Days.Clear();
            for(int i = 0; i < 35; i++)
            {
                Days.Add(_startDate);
                _startDate = _startDate.AddDays(1);
            }

            return Days;
        }
    }
}
