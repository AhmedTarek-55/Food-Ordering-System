using AdminPanel.Models.RoleViewModels;

namespace AdminPanel.Models.UserViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
