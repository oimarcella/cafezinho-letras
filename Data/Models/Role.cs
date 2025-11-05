using Microsoft.AspNetCore.Identity;

namespace CafezinhoELivrosApi.Data.Models
{
    public class Role: IdentityRole
    {
        //public Guid Id { get; set; }
        //public string Name { get; set; }
    }

    public enum ERoles
    {
        Administrador,
        Leitor,
        Contribuidor
    }
}
