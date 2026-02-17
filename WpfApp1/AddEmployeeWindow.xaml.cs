using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Models;
using WpfApp1.Utilities;

namespace WpfApp1
{
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Validation_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtBank == null || txtAccountNumber == null || txtValidationMessage == null) return;

            string bank = txtBank.Text;
            string accNo = txtAccountNumber.Text;

            var result = BankValidator.ValidateAccountNumber(bank, accNo);

            txtValidationMessage.Text = result.Message;
            if (result.IsValid)
            {
                txtValidationMessage.Foreground = Brushes.Green;
            }
            else
            {
                txtValidationMessage.Foreground = Brushes.Red;
            }
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
                    DialogResult = true;
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

            // Optional: Block save if validation fails?
            // User requirement: "indicate if we type less or extra"
            // It doesn't strictly say block, but usually we should warn.
            // I'll stick to warning in the text block for now, but allow save if the user insists, or I can block it.
            // Let's block it if it's invalid (but allow 'unknown' banks).
            var validation = BankValidator.ValidateAccountNumber(txtBank.Text, txtAccountNumber.Text);
            if (!validation.IsValid)
            {
                 // Confirm with user? Or just block.
                 // "jit should indicate" implies indication.
                 // I will allow save but show a warning dialog if invalid.
                 var result = MessageBox.Show($"Bank Account Number validation warning: {validation.Message}\nDo you want to proceed?", "Validation Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                 if (result == MessageBoxResult.No) return false;
            }

            return true;
        }

        private bool ShowError(string message)
        {
            MessageBox.Show(message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
    }
}
