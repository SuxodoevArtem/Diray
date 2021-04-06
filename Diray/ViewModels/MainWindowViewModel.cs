using Diray.Data;
using Diray.Model;
using Diray.Servise;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;

namespace Diray.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        readonly ParsWeather weather = new ParsWeather();
        readonly Сalendar _сalendar;
        readonly ConnectionProvider connectionProvider;

        private string _temperature = "";
        private string _icon = "";
        private string _currentDate = DateTime.Now.ToString("yyyy MMMM");
        private List<DateTime> _days = new List<DateTime>();
        private Day _selectedDay;
        private Note _selectedNote;
        private Day _currentDay;
        private string _title = "Введите заголовок";
        private string _content = "Введите заметку";

        public ObservableCollection<Day> Days { get; set; }
        SQLCommands datasetInitializer;

        public MainWindowViewModel()
        {
            Wheather();

            using (connectionProvider = new ConnectionProvider(
                @$"DataSource=C:\Програмирование\Проекты\Diray\Diray\DB\MyDb.db;Version=3"));

            datasetInitializer = new SQLCommands(connectionProvider);
            datasetInitializer.InitSchema();

            _сalendar = new Сalendar(DateTime.Parse(_currentDate));
            _days = _сalendar.GetDays();

            Days = new ObservableCollection<Day>();

            InitDays();
            
        }

        #region Weather
        public string Temperature {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        public string Icon {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged("Icon");
            }
        }
        #endregion

        #region Date
        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        public Day SelectedDay
        {
            get => _selectedDay;
            set
            {
                Title = "Введите заголовок";
                Content = "Введите заметку";
                _selectedDay = value;
                OnPropertyChanged("SelectedDay");
            }
        }

        public Note SelectedNote
        {
            get => _selectedNote;
            set
            {
                _selectedNote = value;
                OnPropertyChanged("SelectedNote");
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        #endregion

        public async Task Wheather()
        {
            await weather.Request();

            Temperature = weather.temp;
            Icon = weather.iconUrl;
            OnPropertyChanged("Temperature");
            OnPropertyChanged("Icon");
        }

        #region Commands

        Command nextMonthCommand;
        Command lastMonthCommand;
        Command updateCommand;
        Command addCommand;
        Command deleteCommand;

        public Command NextMonthCommand
        {
            get
            {
                return nextMonthCommand ?? (nextMonthCommand = new Command(obj =>
                {
                    CurrentDate = DateTime.Parse(_currentDate).AddMonths(1).ToString("yyyy MMMM");
                    OnPropertyChanged("CurrentDate");
                    _сalendar.AddMonth(DateTime.Parse(_currentDate));
                    _days = _сalendar.GetDays();

                    InitDays();
                }));
            }
        }

        public Command LastMonthCommand
        {
            get
            {
                return lastMonthCommand ?? (lastMonthCommand = new Command(obj =>
                {
                    CurrentDate = DateTime.Parse(_currentDate).AddMonths(-1).ToString("yyyy MMMM");
                    OnPropertyChanged("CurrentDate");
                    _сalendar.AddMonth(DateTime.Parse(_currentDate));
                    _days = _сalendar.GetDays();

                    InitDays();
                }));
            }
        }

        public Command AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new Command(obj =>
                {
                    DateTime selected = SelectedDay.Date;
                    datasetInitializer.AddNote(SelectedDay.Date.ToString("yyyy-MM-dd"), Title, Content);

                    InitDays();

                    Title = "Введите заголовок";
                    Content = "Введите заметку";

                    SelectedDay = Days.Where(item => item.DateString == selected.ToString("dd MMMM yyyy")).First();

                }));
            }
        }
        public Command UpdateCommand
        {
            get
            {
                return updateCommand ?? (updateCommand = new Command(obj =>
                {
                    if (SelectedNote != null)
                    {
                        datasetInitializer.UpdateNote(SelectedNote.id, SelectedNote.Title, SelectedNote.Content);
                    }
                
                }));
            }
        }

        public Command DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new Command(obj =>
                {
                    DateTime selected = SelectedDay.Date;
                    if (SelectedNote != null)
                    {
                        datasetInitializer.DeletNote(SelectedDay.DayId, SelectedNote.id, SelectedDay.listNotes.Count());
                    }

                    InitDays();

                    SelectedDay = Days.Where(item => item.DateString == selected.ToString("dd MMMM yyyy")).First();
                }));
            }
        }

        #endregion

        public void InitDays()
        {
           
            var data = datasetInitializer.GetDayOfMonth(_сalendar.currentMonth);
            
            Days.Clear();
            foreach (var n in _days)
            {
                Days.Add(new Day { date = n});
                foreach (var j in data)
                {
                    if (n.ToString("yyyy-MM-dd") == j.Date)
                    {
                        var notes = datasetInitializer.GetNoteOfDay(j.Id);
                        ObservableCollection<Note> listnotes = new ObservableCollection<Note>();

                        Days.RemoveAt(Days.Count - 1);
                        notes.ForEach(item => listnotes.Add(new Note(item.Id, item.Title, item.Content, UpdateCommand, DeleteCommand)));
                        Days.Add(new Day { 
                            date = n,
                            dayId = j.Id,
                            ListNotes = listnotes
                        });
                    }

                }
            }

            _currentDay = _currentDay ?? Days.Where(item => item.DateString == DateTime.Now.Date.ToString("dd MMMM yyyy")).First();
            SelectedDay = _currentDay;

        }

        public string FormatString(string str)
        {
            return str;
        }
    }
}
