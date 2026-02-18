using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class AppLoginWindow : Window
    {
        private readonly DatabaseConfigService _dbConfigService;

        public AppLoginWindow()
        {
            InitializeComponent();
            _dbConfigService = new DatabaseConfigService();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckDatabaseConnection();
        }

        private void CheckDatabaseConnection()
        {
            txtConnectionStatus.Text = "Checking connection...";
            txtConnectionStatus.Foreground = Brushes.Gray;
            btnLogin.IsEnabled = false;

            try
            {
                using (var context = new PayBillDbContext())
                {
                    if (context.Database.CanConnect())
                    {
                        // Try to ensure database is created if it doesn't exist
                        context.Database.EnsureCreated();

                        // Check if the logins table exists/works
                        try
                        {
                            var count = context.Logins.Count();
                            txtConnectionStatus.Text = "Connected to Database";
                            txtConnectionStatus.Foreground = Brushes.Green;
                            btnLogin.IsEnabled = true;
                        }
                        catch (Exception ex) when (ex.Message.Contains("doesn't exist") || ex.InnerException?.Message.Contains("doesn't exist") == true)
                        {
                            // If table is missing despite EnsureCreated (e.g. existing DB without table), create it manually
                            try
                            {
                                string createTableSql = @"
                                    CREATE TABLE IF NOT EXISTS `logins` (
                                      `id` INT NOT NULL AUTO_INCREMENT,
                                      `username` VARCHAR(50) NOT NULL,
                                      `password` VARCHAR(255) NOT NULL,
                                      `role` VARCHAR(50) NULL,
                                      PRIMARY KEY (`id`),
                                      UNIQUE INDEX `username_UNIQUE` (`username` ASC) VISIBLE)
                                    ENGINE = InnoDB
                                    DEFAULT CHARACTER SET = utf8mb4
                                    COLLATE = utf8mb4_0900_ai_ci;";

                                string insertAdminSql = @"
                                    INSERT INTO `logins` (`username`, `password`, `role`)
                                    VALUES ('Admin', '123456', 'admin')
                                    ON DUPLICATE KEY UPDATE `password` = '123456';";

                                context.Database.ExecuteSqlRaw(createTableSql);
                                context.Database.ExecuteSqlRaw(insertAdminSql);

                                txtConnectionStatus.Text = "Connected (Table 'logins' created)";
                                txtConnectionStatus.Foreground = Brushes.Green;
                                btnLogin.IsEnabled = true;
                            }
                            catch (Exception createEx)
                            {
                                txtConnectionStatus.Text = $"Failed to create table: {createEx.Message}";
                                txtConnectionStatus.Foreground = Brushes.Red;
                            }
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
                     txtConnectionStatus.Text = "Access Denied: Check username/password in Database Settings.";
                }
                else
                {
                     txtConnectionStatus.Text = $"Connection Failed: {ex.Message}";
                }
                txtConnectionStatus.Foreground = Brushes.Red;
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
                        MainDashboardWindow mainWindow = new MainDashboardWindow();
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

        private void BtnToggleSettings_Click(object sender, RoutedEventArgs e)
        {
            pnlLogin.Visibility = Visibility.Collapsed;
            pnlSettings.Visibility = Visibility.Visible;
            btnToggleSettings.Visibility = Visibility.Collapsed;

            // Load current settings
            var builder = _dbConfigService.GetConnectionStringBuilder();
            txtDbServer.Text = builder.Server;
            txtDbName.Text = builder.Database;
            txtDbUser.Text = builder.UserID;
            txtDbPassword.Password = builder.Password;
        }

        private void BtnCancelSettings_Click(object sender, RoutedEventArgs e)
        {
            pnlSettings.Visibility = Visibility.Collapsed;
            pnlLogin.Visibility = Visibility.Visible;
            btnToggleSettings.Visibility = Visibility.Visible;
        }

        private void BtnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _dbConfigService.SaveConnectionString(
                    txtDbServer.Text,
                    txtDbName.Text,
                    txtDbUser.Text,
                    txtDbPassword.Password
                );

                MessageBox.Show("Configuration saved successfully. Testing connection...", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Switch back to login
                pnlSettings.Visibility = Visibility.Collapsed;
                pnlLogin.Visibility = Visibility.Visible;
                btnToggleSettings.Visibility = Visibility.Visible;

                // Test connection
                CheckDatabaseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
