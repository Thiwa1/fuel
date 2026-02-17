using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class ScheduleListView : UserControl
    {
        public ScheduleListView()
        {
            InitializeComponent();
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
    }
}
