using System;
using System.Collections.Generic;
using System.Text;
using Diray.ViewModels;

namespace Diray.Model
{
    class Day : ViewModel
    {
        public DateTime date;
        public List<Note> listNotes = new List<Note>();
        public int dayId;
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public int DayId
        {
            get => dayId;
            set
            {
                dayId = value;
                OnPropertyChanged("DayId");
            }
        }

        public List<Note> ListNotes
        {
            get => listNotes;
            set
            {
                listNotes = value;
                OnPropertyChanged("ListNotes");
            }
        }
    }
}
