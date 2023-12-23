using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.ProductBrandViewModels
{
    public class ProductBrandViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand Name is Required")]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
