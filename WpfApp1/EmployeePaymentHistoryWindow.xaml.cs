using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class EmployeePaymentHistoryWindow : Window
    {
        private int _employeeId;

        public EmployeePaymentHistoryWindow(int employeeId)
        {
            InitializeComponent();
            _employeeId = employeeId;
            LoadHistory();
        }

        private void LoadHistory()
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    var employee = context.Employees.Find(_employeeId);
                    if (employee != null)
                    {
                        txtEmployeeName.Text = $"{employee.CallingName} - Payment History";
                    }

                    var payments = context.Payments
                        .Include(p => p.Schedule)
                        .Where(p => p.EmployeeId == _employeeId)
                        .OrderByDescending(p => p.PaymentDate)
                        .ToList();

                    dgHistory.ItemsSource = payments;

                    var total = payments.Sum(p => p.Amount);
                    txtTotalPaid.Text = total.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
