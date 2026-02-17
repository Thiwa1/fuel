using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("schedule_id")]
        public int? ScheduleId { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("payment_date", TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee? Employee { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        public virtual Schedule? Schedule { get; set; }
    }
}
