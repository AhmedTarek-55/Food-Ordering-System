using Microsoft.AspNetCore.Identity;

namespace Core.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}