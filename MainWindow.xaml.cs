using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace NotepadClone
{
    public partial class MainWindow : Window
    {
        private string _fileName = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Choose a text file... | *.txt";

            if (fileDialog.ShowDialog() == true)
            {
                _fileName = fileDialog.FileName;

                UpdateStatus();

                btnRead_Click(sender, e);
            }
            else
                tbContent.Text = "A file has not been selected :(";
        }

        private void UpdateStatus()
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                btnExit.IsEnabled = false;
                btnRedact.IsEnabled = false;
                btnRead.IsEnabled = false;

                btnSelect.IsEnabled = true;
            }
            else
            {
                btnExit.IsEnabled = true;
                btnRedact.IsEnabled = true;
                btnRead.IsEnabled = true;

                btnSelect.IsEnabled = false;
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

            tbContent_LostFocus(sender, e);

            UpdateStatus();
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            tbContent.Visibility = Visibility.Visible;
            tbContent.IsEnabled = true;

            try
            {
                using (var streamReader = new StreamReader(_fileName))
                {
                    tbContent.Text = streamReader.ReadToEnd();
                }
            }
            catch
            {
                MessageBox.Show("Could not read the file :(", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

                tbContent_LostFocus(sender, e);
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
            tbContent.IsEnabled = false;
        }
    }
}
