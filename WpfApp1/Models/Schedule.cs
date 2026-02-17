using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("schedules")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [Column("date", TypeName = "date")]
        public DateTime Date { get; set; }

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedAt { get; set; }
    }
}
