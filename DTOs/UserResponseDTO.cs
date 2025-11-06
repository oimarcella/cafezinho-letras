using CafezinhoELivrosApi.Data.Models;

namespace CafezinhoELivrosApi.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string CurrentThought { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Role Role { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
