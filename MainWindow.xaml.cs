using Microsoft.Win32;
using NotepadClone.View.UserControls;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace NotepadClone
{
    public partial class MainWindow : Window
    {
        private string _fileName = "";

        public MainWindow()
        {
            DataContext = this;

            SetListView();

            InitializeComponent();
        }

        private ObservableCollection<FileItem> _file = new ObservableCollection<FileItem>();

        public ObservableCollection<FileItem> Files
        {
            get { return _file; }
            set { _file = value; }
        }

        private void SetListView()
        {
            foreach (string fileName in Directory.GetFiles(@"C:\Getting_Started\VS\NotepadClone\Files\"))
            {
                Files.Add(new FileItem() { Path = fileName, CreationDate = File.GetCreationTime(fileName) });
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (lvFiles.SelectedItem == null)
                MessageBox.Show("No file selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            
            else
            {
                var fileItem = (FileItem)lvFiles.SelectedItem;

                _fileName = fileItem.Path;

                UpdateStatus();

                btnRead_Click(sender, e);
            }
        }

        private void UpdateStatus()
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                btnExit.IsEnabled = false;
                btnRedact.IsEnabled = false;
                btnRead.IsEnabled = false;

                btnSelect.IsEnabled = true;
                lvFiles.Visibility = Visibility.Visible;
            }
            else
            {
                btnExit.IsEnabled = true;
                btnRedact.IsEnabled = true;
                btnRead.IsEnabled = true;

                btnSelect.IsEnabled = false;
                lvFiles.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            tbContent_LostFocus(sender, e);
            tboxEditing.Visibility = Visibility.Visible;

            using (var streamReader = new StreamReader(_fileName))
            {
                tboxEditing.Text = streamReader.ReadToEnd();
            }

            tboxEditing.Focus();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            _fileName = "";

            UpdateStatus();
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            tbContent.Visibility = Visibility.Visible;

            try
            {
                using (var streamReader = new StreamReader(_fileName))
                {
                    tbContent.Text = streamReader.ReadToEnd();
                }
            }
            catch
            {
                tbContent_LostFocus(sender, e);

                MessageBox.Show("Could not read the file :(", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void tboxEditing_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Do you want to apply the changes?", "Unsaved changes",
                MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                try
                {
                    File.WriteAllText(_fileName, tboxEditing.Text);
                }
                catch
                {
                    MessageBox.Show("Could not write to a file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            tboxEditing.Clear();
            tboxEditing.Visibility = Visibility.Collapsed;
        }

        private void tbContent_LostFocus(object sender, RoutedEventArgs e)
        {
            tbContent.Text = "";
            tbContent.Visibility = Visibility.Collapsed;
        }
    }
}
