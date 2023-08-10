using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Travels.Models.EF
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int contactID { get; set; }
        [Required(ErrorMessage = "Name cannot be null")]
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email cannot be null")]
        [StringLength(150, ErrorMessage = "Must not exceed 150 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Subject cannot be null")]
        [StringLength(150, ErrorMessage = "Must not exceed 250 characters")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Message cannot be null")]
        public string Message { get; set; }    
        public DateTime? CreateDate { get; set; }
        public bool? IsRead { get; set; }
    }
}
