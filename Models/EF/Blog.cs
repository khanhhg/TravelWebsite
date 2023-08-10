using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Travels.Models.EF
{
    [Table("Blog")]
    public class Blog
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        [Required(ErrorMessage = "Title cannot be null")]
        [StringLength(250, ErrorMessage = "Must not exceed 250 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description cannot be null")]
        [StringLength(500, ErrorMessage = "Must not exceed 500 characters")]
        public string Description { get; set; }

        [DisplayFormat(HtmlEncode = true)]
        [Required(ErrorMessage = "Detail cannot be null")]
        public string Detail { get; set; }
        public string Image { get; set; }      
        public bool IsActive { get; set; }
    }
}
