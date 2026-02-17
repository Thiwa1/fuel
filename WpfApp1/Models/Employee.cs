using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("calling_name")]
        [StringLength(100)]
        public required string CallingName { get; set; }

        [Required]
        [Column("account_name")]
        [StringLength(100)]
        public required string AccountName { get; set; }

        [Required]
        [Column("employee_number")]
        [StringLength(50)]
        public required string EmployeeNumber { get; set; }

        [Required]
        [Column("bank")]
        [StringLength(100)]
        public required string Bank { get; set; }

        [Required]
        [Column("branch")]
        [StringLength(100)]
        public required string Branch { get; set; }

        [Required]
        [Column("nic_no")]
        [StringLength(50)]
        public required string NicNo { get; set; }

        [Required]
        [Column("account_number")]
        [StringLength(50)]
        public required string AccountNumber { get; set; }

        [Column("area")]
        [StringLength(100)]
        public string? Area { get; set; }
    }
}
