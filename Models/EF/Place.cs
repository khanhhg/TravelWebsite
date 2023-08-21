using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;
using Travels.Models.Common;

namespace Travels.Models.EF
{
    [Table("Place")]
    public class Place :CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PlaceId { get; set; }
        [Required(ErrorMessage = "PlaceName cannot be null")]
        [StringLength(250, ErrorMessage = "Must not exceed 250 characters")]
        public string PlaceName { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public ICollection<Tour> Tours { get; set; }
    }
}
