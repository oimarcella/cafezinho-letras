using CafezinhoELivrosApi.Data;
using CafezinhoELivrosApi.Data.Models;
using CafezinhoELivrosApi.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafezinhoELivrosApi.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<Role>  _roleManager;

        public AccountController(AppDbContext context, RoleManager<Role> roleManager) {
            _dbContext = context;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        ///<remarks>
        /// Cria um usuário com a role padrão de Leitor.
        ///</remarks>
        /// <param name="user">Dados do usuário a ser criado.</param>
        /// <returns>Retorna o usuário criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Index([FromBody] CreateUserDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Usuário informado inválido");

                if (await _dbContext.Users.AnyAsync(x => x.UserName == user.UserName || x.Email == user.Email))
                    return BadRequest("Já existe uma conta com esses nome de usuário/email");

                var newUser = new User
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    BirthDate = user.BirthDate,
                    CurrentThought = user.CurrentThought,
                    City = user.City,
                    State = user.State,
                    Description = user.Description,
                    RoleId = await _roleManager.Roles.Where(r => r.Name == ERoles.Leitor.ToString()).Select(r => r.Id).FirstOrDefaultAsync()
                };

                var result = await _dbContext.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                // Ver AutoMapper
                var response = new UserResponseDTO
                {
                    Id = new Guid(newUser.Id),
                    Name = newUser.Name,
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    BirthDate = newUser.BirthDate,
                    CreatedAt = newUser.CreatedAt,
                    CurrentThought = newUser.CurrentThought,
                    Description = newUser.Description,
                    City = newUser.City,
                    State = newUser.State,
                    Role = newUser.Role,
                }; 

                return Ok(response);
            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Deleta a conta de um usuário
        /// </summary>
        /// <param name="id">Id da conta que vai ser deletada</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {

            if (id == null)
                return BadRequest("Id de usuário informado inválido.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return BadRequest("Usuário não existe.");

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok("A conta foi deletada!");
        }
    }
}
