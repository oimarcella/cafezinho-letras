using System.ComponentModel.DataAnnotations;

namespace CafezinhoELivrosApi.DTOs
{
    public class UpdateUserPartialDTO
    {
        [MaxLength(60)]
        [MinLength(3)]
        public string? Name {  get; set; }

        [MaxLength(30)]
        [MinLength(3)]
        public string? UserName {  get; set; }

        [MaxLength(60)]
        public string? CurrentThought {  get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [MaxLength(30)]
        [MinLength(3)]
        public string? City { get; set; }

        [MaxLength(30)]
        [MinLength(3)]
        public string? State { get; set; }
    }
}
