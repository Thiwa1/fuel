using System.Windows;
using System.Windows.Controls;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowDashboard();
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            ShowDashboard();
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Employees";
            contentArea.Content = new EmployeeListView();
        }

        private void BtnPayments_Click(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Payments (Excel Upload)";
            contentArea.Content = new PaymentUploadView();
        }

        private void BtnSchedules_Click(object sender, RoutedEventArgs e)
        {
            txtPageTitle.Text = "Schedules";
            contentArea.Content = new ScheduleListView();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void ShowDashboard()
        {
            txtPageTitle.Text = "Dashboard";
            // Simple dashboard content for now
            StackPanel dashboard = new StackPanel();
            dashboard.Children.Add(new TextBlock { Text = "Welcome to Pay Bill System Pro", FontSize = 20, Foreground = System.Windows.Media.Brushes.Gray });
            dashboard.Children.Add(new TextBlock { Text = "Select an option from the menu to get started.", FontSize = 16, Margin = new Thickness(0, 10, 0, 0) });

            contentArea.Content = dashboard;
        }
    }
}
