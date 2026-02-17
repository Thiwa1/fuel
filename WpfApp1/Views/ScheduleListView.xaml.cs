using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class ScheduleListView : UserControl
    {
        private Action<int> _navigateToDetails;

        public ScheduleListView(Action<int> navigateToDetails = null)
        {
            InitializeComponent();
            _navigateToDetails = navigateToDetails;
            LoadSchedules();
        }

        private void LoadSchedules()
        {
            using (var context = new PayBillDbContext())
            {
                var schedules = context.Schedules.OrderByDescending(s => s.Date).ToList();
                dgSchedules.ItemsSource = schedules;
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSchedules();
        }

        private void BtnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddScheduleWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadSchedules();
            }
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (_navigateToDetails != null && sender is Button btn && btn.DataContext is Schedule schedule)
            {
                _navigateToDetails(schedule.Id);
            }
        }
    }
}
