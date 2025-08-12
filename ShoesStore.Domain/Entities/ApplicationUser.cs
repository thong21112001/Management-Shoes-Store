using Microsoft.AspNetCore.Identity;

namespace ShoesStore.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public byte[]? UserImage { get; set; }
    }
}
