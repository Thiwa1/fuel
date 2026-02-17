using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using MiniExcelLibs;
using WpfApp1.Data;
using WpfApp1.Models;
using Microsoft.Win32;
using System.Collections.Generic;

namespace WpfApp1.Views
{
    public partial class ScheduleDetailsView : UserControl
    {
        private int _scheduleId;
        private Action _goBack;

        public ScheduleDetailsView(int scheduleId, Action goBack)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            _goBack = goBack;
            LoadPayments();
        }

        private void LoadPayments(string searchQuery = "")
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    var query = context.Payments
                        .Include(p => p.Employee)
                        .Where(p => p.ScheduleId == _scheduleId);

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        searchQuery = searchQuery.ToLower();
                        query = query.Where(p => p.Employee.CallingName.ToLower().Contains(searchQuery) ||
                                                 p.Employee.EmployeeNumber.ToLower().Contains(searchQuery) ||
                                                 p.Employee.AccountName.ToLower().Contains(searchQuery));
                    }

                    var payments = query.OrderBy(p => p.Employee.CallingName).ToList();
                    dgPayments.ItemsSource = payments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payments: {ex.Message}");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _goBack?.Invoke();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadPayments(txtSearch.Text);
        }

        private void btnAddPayment_Click(object sender, RoutedEventArgs e)
        {
             var addWindow = new AddSinglePaymentWindow(_scheduleId);
             if (addWindow.ShowDialog() == true)
             {
                 LoadPayments(txtSearch.Text);
             }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPayments.SelectedItem is Payment selectedPayment)
            {
                if (MessageBox.Show("Are you sure you want to delete this payment?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var context = new PayBillDbContext())
                        {
                            var payment = context.Payments.Find(selectedPayment.Id);
                            if (payment != null)
                            {
                                context.Payments.Remove(payment);
                                context.SaveChanges();
                                LoadPayments(txtSearch.Text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting payment: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a payment to delete.");
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = $"Schedule_{_scheduleId}_Payments_{DateTime.Now:yyyyMMdd}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // We need to flatten the data for export and match the requested columns
                    var payments = (System.Collections.IEnumerable)dgPayments.ItemsSource;
                    var exportList = new List<object>();

                    foreach (Payment p in payments)
                    {
                        exportList.Add(new
                        {
                            CallingName = p.Employee?.CallingName,
                            EmpNo = p.Employee?.EmployeeNumber,
                            AccountName = p.Employee?.AccountName,
                            Bank = p.Employee?.Bank,
                            Branch = p.Employee?.Branch,
                            // Ensure Account No is treated as string to keep leading zeros
                            AccountNo = p.Employee?.AccountNumber,
                            Area = p.Employee?.Area,
                            Amount = p.Amount
                        });
                    }

                    MiniExcel.SaveAs(saveFileDialog.FileName, exportList);
                    MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
