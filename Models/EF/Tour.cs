using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Travels.Models.Common;

namespace Travels.Models.EF
{
    [Table("Tour")]
    public class Tour : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TourId { get; set; }
        [Required(ErrorMessage = "Destination cannot be null")]
        [StringLength(250, ErrorMessage = "Must not exceed 250 characters")]
        public string Destination { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public int PlaceId { get; set; }
        public virtual Place Place { get; set; }
    }
}
