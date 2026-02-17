using System;
using System.Linq;
using System.Windows;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class AddSinglePaymentWindow : Window
    {
        private int _scheduleId;

        public AddSinglePaymentWindow(int scheduleId)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            LoadEmployees();
            dpDate.SelectedDate = DateTime.Today;
        }

        private void LoadEmployees()
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    var employees = context.Employees.OrderBy(e => e.CallingName).ToList();
                    cmbEmployees.ItemsSource = employees;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmployees.SelectedValue == null)
            {
                MessageBox.Show("Please select an employee.");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.");
                return;
            }

            try
            {
                using (var context = new PayBillDbContext())
                {
                    var payment = new Payment
                    {
                        ScheduleId = _scheduleId,
                        EmployeeId = (int)cmbEmployees.SelectedValue,
                        Amount = amount,
                        PaymentDate = dpDate.SelectedDate.Value
                    };

                    context.Payments.Add(payment);
                    context.SaveChanges();

                    MessageBox.Show("Payment added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payment: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
