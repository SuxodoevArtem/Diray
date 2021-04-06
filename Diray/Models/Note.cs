using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Diray.ViewModels;

namespace Diray.Model
{
    class Note : ViewModel
    {
        private string title;
        private string content;
        public int id;
        public Command UpdateCommand { get; set; }
        public Command DeleteCommand { get; set; }

        public Note( int id, string title, string content, Command updateCommand, Command deleteCommand)
        {
            this.id = id;

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            this.title = title;

            this.content = content;
            UpdateCommand = updateCommand;
            DeleteCommand = deleteCommand;

        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

    }
}
