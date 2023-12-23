using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.RoleViewModels
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage = "Role Name is Required")]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
