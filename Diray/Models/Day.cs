using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Diray.ViewModels;

namespace Diray.Model
{
    class Day : ViewModel
    {
        public DateTime date;
        public ObservableCollection<Note> listNotes = new ObservableCollection<Note>();
        public int dayId;
        public string DateString
        {
            get => date.ToString("dd MMMM yyyy");
        }

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

        public ObservableCollection<Note> ListNotes
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
