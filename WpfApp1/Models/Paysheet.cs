using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("paysheets")]
    public class Paysheet
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("schedule_id")]
        public int? ScheduleId { get; set; }

        [Required]
        [Column("date", TypeName = "date")]
        public DateTime Date { get; set; }

        [Column("emp_code")]
        [StringLength(50)]
        public string? EmpCode { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("designation")]
        [StringLength(100)]
        public string? Designation { get; set; }

        [Column("working_place")]
        [StringLength(100)]
        public string? WorkingPlace { get; set; }

        [Column("project_head")]
        [StringLength(100)]
        public string? ProjectHead { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("basic_salary", TypeName = "decimal(10, 2)")]
        public decimal? BasicSalary { get; set; }

        [Column("travelling_allowance", TypeName = "decimal(10, 2)")]
        public decimal? TravellingAllowance { get; set; }

        [Column("vehicle_allowance", TypeName = "decimal(10, 2)")]
        public decimal? VehicleAllowance { get; set; }

        [Column("arrears", TypeName = "decimal(10, 2)")]
        public decimal? Arrears { get; set; }

        [Column("gross_pay", TypeName = "decimal(10, 2)")]
        public decimal? GrossPay { get; set; }

        [Column("salary_nopay_days", TypeName = "decimal(10, 2)")]
        public decimal? SalaryNopayDays { get; set; }

        [Column("nopay_budgetory", TypeName = "decimal(10, 2)")]
        public decimal? NopayBudgetory { get; set; }

        [Column("nopay_other", TypeName = "decimal(10, 2)")]
        public decimal? NopayOther { get; set; }

        [Column("epf_8", TypeName = "decimal(10, 2)")]
        public decimal? Epf8 { get; set; }

        [Column("salary_advance", TypeName = "decimal(10, 2)")]
        public decimal? SalaryAdvance { get; set; }

        [Column("staff_loan", TypeName = "decimal(10, 2)")]
        public decimal? StaffLoan { get; set; }

        [Column("communication_deduction", TypeName = "decimal(10, 2)")]
        public decimal? CommunicationDeduction { get; set; }

        [Column("net_pay", TypeName = "decimal(10, 2)")]
        public decimal? NetPay { get; set; }

        [Column("hold")]
        [StringLength(50)]
        public string? Hold { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        public virtual Schedule? Schedule { get; set; }
    }
}
