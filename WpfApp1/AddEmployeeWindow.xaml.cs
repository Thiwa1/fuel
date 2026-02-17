using System;
using System.Windows;
using WpfApp1.Data;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (var context = new PayBillDbContext())
                {
                    var newEmployee = new Employee
                    {
                        CallingName = txtCallingName.Text,
                        AccountName = txtAccountName.Text,
                        EmployeeNumber = txtEmployeeNumber.Text,
                        Bank = txtBank.Text,
                        Branch = txtBranch.Text,
                        NicNo = txtNicNo.Text,
                        AccountNumber = txtAccountNumber.Text,
                        Area = txtArea.Text
                    };

                    context.Employees.Add(newEmployee);
                    context.SaveChanges();

                    MessageBox.Show("Employee added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving employee: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCallingName.Text)) return ShowError("Calling Name is required.");
            if (string.IsNullOrWhiteSpace(txtAccountName.Text)) return ShowError("Account Name is required.");
            if (string.IsNullOrWhiteSpace(txtEmployeeNumber.Text)) return ShowError("Employee Number is required.");
            if (string.IsNullOrWhiteSpace(txtBank.Text)) return ShowError("Bank is required.");
            if (string.IsNullOrWhiteSpace(txtBranch.Text)) return ShowError("Branch is required.");
            if (string.IsNullOrWhiteSpace(txtNicNo.Text)) return ShowError("NIC No is required.");
            if (string.IsNullOrWhiteSpace(txtAccountNumber.Text)) return ShowError("Account Number is required.");
            return true;
        }

        private bool ShowError(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
    }
}
