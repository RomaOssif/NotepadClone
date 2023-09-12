using System.Windows.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NotepadClone.View.UserControls
{
    public partial class FileItem : UserControl, INotifyPropertyChanged
    {
        public FileItem()
        {
            DataContext = this;

            InitializeComponent();
        }

        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                Title = System.IO.Path.GetFileNameWithoutExtension(value);
                path = value;

                OnPropertyChanged();
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;

                OnPropertyChanged();
            }
        }

        private DateTime creationDate;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;

                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged( [CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
