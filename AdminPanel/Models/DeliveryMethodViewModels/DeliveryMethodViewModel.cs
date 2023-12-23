using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.DeliveryMethodViewModels
{
    public class DeliveryMethodViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Delivery Time is Required")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
