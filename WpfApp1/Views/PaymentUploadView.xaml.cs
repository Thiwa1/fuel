using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MiniExcelLibs;
using WpfApp1.Data;
using WpfApp1.Models;
using WpfApp1.ViewModels;
using System.IO;

namespace WpfApp1.Views
{
    public partial class PaymentUploadView : UserControl
    {
        private List<PaymentPreviewItem> _previewItems = new List<PaymentPreviewItem>();

        public PaymentUploadView()
        {
            InitializeComponent();
            LoadSchedules();
            dpPaymentDate.SelectedDate = DateTime.Today;
        }

        public void LoadSchedules()
        {
            try
            {
                using (var context = new PayBillDbContext())
                {
                    cmbSchedules.ItemsSource = context.Schedules.OrderByDescending(s => s.Date).ToList();
                    if (cmbSchedules.Items.Count > 0)
                        cmbSchedules.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                 MessageBox.Show($"Error loading schedules: {ex.Message}");
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls;*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    ProcessFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ProcessFile(string filePath)
        {
            _previewItems.Clear();

            // Read Excel without headers, assuming Column A is EmpNo, Column B is Amount
            var rows = MiniExcel.Query(filePath, useHeaderRow: false).ToList();

            using (var context = new PayBillDbContext())
            {
                var employees = context.Employees.ToDictionary(e => e.EmployeeNumber, e => e);

                foreach (IDictionary<string, object> row in rows)
                {
                    // MiniExcel returns IDictionary<string, object> with keys "A", "B", etc. when useHeaderRow is false

                    string colA = row.ContainsKey("A") && row["A"] != null ? row["A"].ToString() : "";
                    string colB = row.ContainsKey("B") && row["B"] != null ? row["B"].ToString() : "";

                    if (string.IsNullOrWhiteSpace(colA) && string.IsNullOrWhiteSpace(colB))
                        continue;

                    // Simple heuristic to skip header row
                    if (colA.ToLower().Contains("emp") || colB.ToLower().Contains("amount"))
                        continue;

                    if (!decimal.TryParse(colB, out decimal amount))
                    {
                        // Invalid amount, check if it's empty
                        if (string.IsNullOrWhiteSpace(colB)) amount = 0;
                        else
                        {
                             _previewItems.Add(new PaymentPreviewItem
                            {
                                EmployeeNumber = colA,
                                Amount = 0,
                                Status = "Invalid Amount"
                            });
                            continue;
                        }
                    }

                    if (employees.TryGetValue(colA, out var emp))
                    {
                        _previewItems.Add(new PaymentPreviewItem
                        {
                            EmployeeNumber = colA,
                            EmployeeId = emp.Id,
                            Name = emp.CallingName,
                            AccountNumber = emp.AccountNumber,
                            Amount = amount,
                            Status = "Ready"
                        });
                    }
                    else
                    {
                         _previewItems.Add(new PaymentPreviewItem
                        {
                            EmployeeNumber = colA,
                            Amount = amount,
                            Status = "Not Found"
                        });
                    }
                }
            }

            dgPreview.ItemsSource = null;
            dgPreview.ItemsSource = _previewItems;

            CalculateTotal();

            btnSave.IsEnabled = _previewItems.Any(i => i.IsValid);
        }

        private void CalculateTotal()
        {
            var total = _previewItems.Where(i => i.IsValid).Sum(i => i.Amount);
            txtTotalAmount.Text = total.ToString("N2");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSchedules.SelectedValue == null)
            {
                MessageBox.Show("Please select a schedule.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpPaymentDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a payment date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int scheduleId = (int)cmbSchedules.SelectedValue;
            DateTime paymentDate = dpPaymentDate.SelectedDate.Value;

            try
            {
                using (var context = new PayBillDbContext())
                {
                    var validItems = _previewItems.Where(i => i.IsValid).ToList();

                    foreach (var item in validItems)
                    {
                        var payment = new Payment
                        {
                            ScheduleId = scheduleId,
                            EmployeeId = item.EmployeeId,
                            Amount = item.Amount,
                            PaymentDate = paymentDate
                        };
                        context.Payments.Add(payment);
                    }

                    context.SaveChanges();
                    MessageBox.Show($"Successfully saved {validItems.Count} payments.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear list or disable save
                    _previewItems.Clear();
                    dgPreview.ItemsSource = null;
                    btnSave.IsEnabled = false;
                    txtTotalAmount.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving to database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
