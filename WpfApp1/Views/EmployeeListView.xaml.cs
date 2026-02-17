using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Models;
using System.Collections.Generic;

namespace WpfApp1.Views
{
    public partial class EmployeeListView : UserControl
    {
        public EmployeeListView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData(string searchQuery = "")
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    var query = context.Employees.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        searchQuery = searchQuery.ToLower();
                        query = query.Where(e => e.CallingName.ToLower().Contains(searchQuery) ||
                                                 e.EmployeeNumber.ToLower().Contains(searchQuery) ||
                                                 e.AccountName.ToLower().Contains(searchQuery) ||
                                                 e.NicNo.ToLower().Contains(searchQuery));
                    }

                    dgEmployees.ItemsSource = query.OrderBy(e => e.CallingName).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEmployeeWindow();
            addWindow.ShowDialog();
            LoadData(txtSearch.Text); // Refresh list after closing
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
             LoadData(txtSearch.Text);
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
             if (sender is Button btn && btn.DataContext is Employee emp)
             {
                 var historyWindow = new EmployeePaymentHistoryWindow(emp.Id);
                 historyWindow.ShowDialog();
             }
        }
    }
}
