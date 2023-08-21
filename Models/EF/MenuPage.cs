using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Travels.Models.Common;

namespace Travels.Models.EF
{
    [Table("MenuPage")]
    public class MenuPage : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MenuPageId { get; set; }
        [Required(ErrorMessage = "Name cannot be null")]
        [StringLength(250, ErrorMessage = "Must not exceed 250 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Link cannot be null")]
        [StringLength(500, ErrorMessage = "Must not exceed 250 characters")]
        public string Link { get; set; }
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int? Position { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
