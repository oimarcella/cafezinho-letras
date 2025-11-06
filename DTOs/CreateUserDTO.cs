using System.ComponentModel.DataAnnotations;

namespace CafezinhoELivrosApi.DTOs
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Informe um nome")]
        [MinLength(3,ErrorMessage = "Nome precisa conter pelo menos 3 letras")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Informe um nome de usuário")]
        [MinLength(3, ErrorMessage = "Nome de usuário precisa conter pelo menos 3 letras")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Informe um e-mail")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe uma data de nascimento")]
        public DateTime BirthDate { get; set; }
        
        public string? Description { get; set; }
        public string? CurrentThought {  get; set; }
        public string? City {  get; set; }
        public string? State {  get; set; }
    }
}
