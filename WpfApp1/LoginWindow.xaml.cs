using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
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
                        // Try to ensure database is created if it doesn't exist
                        context.Database.EnsureCreated();

                        // Check if the logins table exists/works
                        try
                        {
                            var count = context.Logins.Count();
                            txtConnectionStatus.Text = "Connected to Database";
                            txtConnectionStatus.Foreground = Brushes.Green;
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

                                txtConnectionStatus.Text = "Connected to Database (Table 'logins' created)";
                                txtConnectionStatus.Foreground = Brushes.Green;
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
