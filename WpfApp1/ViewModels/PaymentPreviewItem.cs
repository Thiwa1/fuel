namespace WpfApp1.ViewModels
{
    public class PaymentPreviewItem
    {
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Unknown";
        public int EmployeeId { get; set; } // 0 if not found
        public bool IsValid => Status == "Ready";
    }
}
