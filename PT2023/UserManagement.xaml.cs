using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PT2023
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : UserControl
    {
        public static string usersPath;
        public static string presentationPath; 
        public static string usersPathScripts;
        public static string usersPathVideos;
        public static string usersPathLogs;
        public List<string> users;
        public List<string> presentations;
        string tempPath;

        public delegate void ExitEvent(object sender, string x);
        public event ExitEvent exitEvent;

        public UserManagement()
        {
            InitializeComponent();

            userGrid.Visibility = Visibility.Visible;
            presentationGrid.Visibility = Visibility.Collapsed;
            Return.Visibility = Visibility.Visible; // Hide the return button initially

            users = new List<string>();
            presentations = new List<string>();

            GetDirectories();


            usersListBox.ItemsSource = users;
        }

        private void GetDirectories()
        {
            string executingDirectory = Directory.GetCurrentDirectory();
            usersPath = System.IO.Path.Combine(executingDirectory, "Users");

            bool exists = Directory.Exists(usersPath);

            if (!exists)
                Directory.CreateDirectory(usersPath);

            try
            {
                List<string> temp;

                temp = Directory.GetDirectories(usersPath).ToList();
                foreach (string s in temp)
                {
                    int x = s.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
                    users.Add(s.Substring(x + 1));
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void getPresentationsDirectories()
        {
            
            tempPath = System.IO.Path.Combine(usersPath, "Presentations");

            bool exists = System.IO.Directory.Exists(tempPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(tempPath);
            try
            {
                List<string> temp;

                temp = Directory.GetDirectories(tempPath).ToList();
                foreach (string s in temp)
                {
                    int x = s.LastIndexOf("\\");
                    presentations.Add(s.Substring(x + 1));
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void usersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userNameTextBox.Text = (string)usersListBox.SelectedValue;
        }
        private bool IsValidName(string name)
        {
            string pattern = "^[a-zA-Z0-9_-]+$"; // Valid characters: letters, numbers, underscore, and hyphen
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
        }
        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidName(userNameTextBox.Text))
            {
                MessageBox.Show("Invalid characters in the name. Please use only letters, numbers, underscore, and hyphen.");
                return;
            }

            usersPath = System.IO.Path.Combine(usersPath, userNameTextBox.Text);

            bool exists = Directory.Exists(usersPath);
            if (!exists)
            {
                Directory.CreateDirectory(usersPath);
            }

            presentationGrid.Visibility = Visibility.Visible;
            userGrid.Visibility = Visibility.Collapsed;
            getPresentationsDirectories();
            presentationsListBox.ItemsSource = presentations;
            Return.Visibility = Visibility.Visible; // Show the return button
        }

        private void usersListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectUserAndProceed();
            Return.Visibility = Visibility.Visible; // Show the return button
        }

        private void SelectUserAndProceed()
        {
            if (usersListBox.SelectedItem != null)
            {
                userNameTextBox.Text = usersListBox.SelectedItem.ToString();

                // Remove the following line, as usersPath is already set correctly
                // usersPath = Path.Combine(usersPath, userNameTextBox.Text);

                bool exists = Directory.Exists(usersPath);
                if (!exists)
                {
                    Directory.CreateDirectory(usersPath);
                }

                presentationGrid.Visibility = Visibility.Visible;
                userGrid.Visibility = Visibility.Collapsed;
                getPresentationsDirectories();
                presentationsListBox.ItemsSource = presentations;
            }
        }

        private void presentationsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            presentationNameTextBox.Text = (string)presentationsListBox.SelectedValue;
        }

        private void presentationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidName(presentationNameTextBox.Text))
            {
                MessageBox.Show("Invalid characters in the presentation name. Please use only letters, numbers, underscore, and hyphen.");
                return;
            }

            presentationPath = System.IO.Path.Combine(tempPath, presentationNameTextBox.Text);
            usersPathScripts = System.IO.Path.Combine(presentationPath, "Scripts");
            usersPathVideos = System.IO.Path.Combine(presentationPath, "Videos");
            usersPathLogs = System.IO.Path.Combine(presentationPath, "Logs");

            string executingDirectory = Directory.GetCurrentDirectory();
            string scriptsPath = System.IO.Path.Combine(executingDirectory, "Scripts");
            MainWindow.scriptPath = System.IO.Path.Combine(scriptsPath, "Script.txt");

            bool exists = Directory.Exists(presentationPath);
            if (!exists)
            {
                Directory.CreateDirectory(presentationPath);
                Directory.CreateDirectory(usersPathScripts);
                Directory.CreateDirectory(usersPathVideos);
                Directory.CreateDirectory(usersPathLogs);
            }

            userGrid.Visibility = Visibility.Visible;
            presentationGrid.Visibility = Visibility.Collapsed;
            exitEvent(this, "");

            Return.Visibility = Visibility.Visible; // Show the return button
        }

        private void presentationsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectPresentationAndProceed();
            Return.Visibility = Visibility.Visible; // Show the return button
        }

        private void SelectPresentationAndProceed()
        {
            if (presentationsListBox.SelectedItem != null)
            {
                presentationNameTextBox.Text = presentationsListBox.SelectedItem.ToString();
                presentationPath = System.IO.Path.Combine(tempPath, presentationNameTextBox.Text);
                usersPathScripts = System.IO.Path.Combine(presentationPath, "Scripts");
                usersPathVideos = System.IO.Path.Combine(presentationPath, "Videos");
                usersPathLogs = System.IO.Path.Combine(presentationPath, "Logs");

                string executingDirectory = Directory.GetCurrentDirectory();
                string scriptsPath = System.IO.Path.Combine(executingDirectory, "Scripts");
                MainWindow.scriptPath = System.IO.Path.Combine(scriptsPath, "Script.txt");

                bool exists = Directory.Exists(presentationPath);
                if (!exists)
                {
                    Directory.CreateDirectory(presentationPath);
                    Directory.CreateDirectory(usersPathScripts);
                    Directory.CreateDirectory(usersPathVideos);
                    Directory.CreateDirectory(usersPathLogs);
                }

                userGrid.Visibility = Visibility.Visible;
                presentationGrid.Visibility = Visibility.Collapsed;
                exitEvent(this, "");
            }
        }

        #region button animations
        private void selectButton_MouseEnter(object sender, MouseEventArgs e)
        {
            selectButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Go1.png"));
        }
    

        private void selectButton_MouseLeave(object sender, MouseEventArgs e)
        {
            selectButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Go.png"));
        }

        private void presentationButton_MouseEnter(object sender, MouseEventArgs e)
        {
            presentationButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Go1.png"));
        }

        private void presentationButton_MouseLeave(object sender, MouseEventArgs e)
        {
            presentationButtonImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Go.png"));
        }

        private void Return_MouseEnter(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Return1.png"));
        }

        private void Return_MouseLeave(object sender, MouseEventArgs e)
        {
            ReturnImg.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Return.png"));
        }

        #endregion

        public void Return_Click(object sender, RoutedEventArgs e)
        {
            if (presentationGrid.Visibility == Visibility.Visible)
            {
                presentationGrid.Visibility = Visibility.Collapsed;
                userGrid.Visibility = Visibility.Visible;
                Return.Visibility = Visibility.Collapsed; // Hide the return button on the select/enter presentation name screen
            }
            else if (userGrid.Visibility == Visibility.Visible)
            {
                userGrid.Visibility = Visibility.Collapsed;
                presentationGrid.Visibility = Visibility.Collapsed;
                Return.Visibility = Visibility.Collapsed; // Hide the return button on the select/enter name screen
                exitEvent?.Invoke(this, ""); // Raise the exit event to notify the parent control
            }
        }

    }
}
