using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;
using WpfApp1.ViewModels;

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
            try
            {
                using (var context = new PayBillDbContext())
                {
                    // Use projection to calculate total amount for each schedule
                    var schedules = context.Schedules
                        .Select(s => new ScheduleViewModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Date = s.Date,
                            CreatedAt = s.CreatedAt,
                            TotalAmount = context.Payments.Where(p => p.ScheduleId == s.Id).Sum(p => p.Amount)
                        })
                        .OrderByDescending(s => s.Date)
                        .ToList();

                    dgSchedules.ItemsSource = schedules;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading schedules: {ex.Message}");
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
            if (_navigateToDetails != null && sender is Button btn && btn.DataContext is ScheduleViewModel schedule)
            {
                _navigateToDetails(schedule.Id);
            }
        }
    }
}
