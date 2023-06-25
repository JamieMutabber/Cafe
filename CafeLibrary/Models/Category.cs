using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CafeLibrary.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(20)]
        public string? Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100,ErrorMessage ="Value must be 1-100 :)")]
        public int DisplayOrder { get; set; }
    }
}
