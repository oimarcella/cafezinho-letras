using Microsoft.AspNetCore.Identity;

namespace CafezinhoELivrosApi.Data.Models;

public class User: IdentityUser
{
    //public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? CurrentThought { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt{ get; set; } = DateTime.Now;
    public string? City { get; set; }
    public string? State { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }
}
