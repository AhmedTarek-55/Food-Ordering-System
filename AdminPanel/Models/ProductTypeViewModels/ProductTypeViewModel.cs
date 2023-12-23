using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.ProductTypeViewModels
{
	public class ProductTypeViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Type Name is Required")]
		[MaxLength(255)]
		public string Name { get; set; }
	}
}
