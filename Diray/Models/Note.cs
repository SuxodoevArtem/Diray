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

        public Note( string title, string content)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            this.title = title;

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(content));
            this.content = content;
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
