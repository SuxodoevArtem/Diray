using Diray.Data;
using Diray.Model;
using Diray.Servise;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Diray.ViewModels
{
    class MainWindowViewModel : ViewModel
    {

        readonly ParsWeather weather = new ParsWeather();
        readonly Сalendar _сalendar;

        private string _temperature = "";
        private string _icon = "";
        private string _currentDate = DateTime.Now.ToString("yyyy MMMM");
        private List<Day> _days = new List<Day>();

        public MainWindowViewModel()
        {
            Wheather();

            using ConnectionProvider connectionProvider = new ConnectionProvider(
                @$"DataSource=C:\Програмирование\Проекты\Diray\Diray\DB\db.MyDb;Version=3");

            SQLCommands datasetInitializer = new SQLCommands(connectionProvider);
            datasetInitializer.InitSchema();

            _сalendar = new Сalendar(DateTime.Parse(_currentDate));

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

        public Command NextMonthCommand
        {
            get
            {
                return nextMonthCommand ?? (nextMonthCommand = new Command(obj =>
                {
                    CurrentDate = DateTime.Parse(_currentDate).AddMonths(1).ToString("yyyy MMMM");
                    OnPropertyChanged("CurrentDate");
                    _сalendar.AddMonth(DateTime.Parse(_currentDate));

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

                }));
            }
        }
        #endregion

        public void InitNotes()
        {

        }

    }
}
