using System;

namespace WpfApp1.ViewModels
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
