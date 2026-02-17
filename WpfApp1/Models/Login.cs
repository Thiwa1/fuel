using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Models
{
    [Table("logins")]
    public class Login
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [Column("password")]
        [StringLength(255)]
        public required string Password { get; set; }

        [Column("role")]
        [StringLength(50)]
        public string? Role { get; set; }
    }
}
