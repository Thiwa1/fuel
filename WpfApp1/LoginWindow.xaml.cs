using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    if (context.Database.CanConnect())
                    {
                        // Check if the logins table exists by trying to count records
                        try
                        {
                            var count = context.Logins.Count();
                            txtConnectionStatus.Text = "Connected to Database";
                            txtConnectionStatus.Foreground = Brushes.Green;
                        }
                        catch (Exception ex) when (ex.Message.Contains("doesn't exist") || ex.InnerException?.Message.Contains("doesn't exist") == true)
                        {
                            txtConnectionStatus.Text = "Database connected, but 'logins' table missing. Run pay_bill.sql.";
                            txtConnectionStatus.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        txtConnectionStatus.Text = "Not Connected to Database";
                        txtConnectionStatus.Foreground = Brushes.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                // General connection error
                if (ex.Message.Contains("Access denied"))
                {
                     txtConnectionStatus.Text = "Access Denied: Check username/password in appsettings.json";
                }
                else
                {
                     txtConnectionStatus.Text = $"Connection Failed: {ex.Message}";
                }
                txtConnectionStatus.Foreground = Brushes.Red;
                Console.WriteLine(ex.ToString());
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new PayBillDbContext())
                {
                    // In a real application, passwords should be hashed.
                    // Here we are comparing plain text as per the current plan/request scope.
                    var user = context.Logins.FirstOrDefault(u => u.Username == username && u.Password == password);

                    if (user != null)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
