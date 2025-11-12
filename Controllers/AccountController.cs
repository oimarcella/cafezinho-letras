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
        private readonly UserManager<User>  _userManager;
        private ILogger<AccountController> _logger;

        public AccountController(AppDbContext context,
            RoleManager<Role> roleManager, 
            UserManager<User> userManager, 
            ILogger<AccountController> logger) {
            _dbContext = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Atualiza dados parciais do usuário
        /// </summary>
        ///<remarks>
        /// Permite que o usuário altere alguns campos de seu cadastro. Eles são: Name,UserName, Description, CurrentThought, City, State.
        ///</remarks>
        /// <param name="user">Dados do usuário a ser atualizado.</param>
        /// <returns>Retorna o usuário atualizado.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Index([FromBody] UpdateUserPartialDTO user, [FromRoute] string id)
        {
            try
            {
                _logger.LogError("OBA");

                var userFound = await _dbContext.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (userFound == null)
                    return NotFound("Usuário não encontrado.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                userFound.Name = user.Name?? userFound.Name;
                userFound.UserName = user.UserName?? userFound.UserName;
                userFound.CurrentThought = user.CurrentThought?? userFound.CurrentThought;
                userFound.Description = user.Description?? userFound.Description;
                userFound.City = user.City?? userFound.City;
                userFound.State = user.State?? userFound.State;


                var result = await _dbContext.SaveChangesAsync();
                if (result == 0)
                {
                    return BadRequest($"Não foi possível atualizar usuário");
                }
                else if (result > 1){
                    _logger.LogCritical($"\n\n ATENÇÃO - {result} foram atualizados, onde deveria ser somente 1.\n");
                }

                var response = new UserResponseDTO
                {
                    Id = new Guid(userFound.Id),
                    Name = userFound.Name,
                    UserName = userFound.UserName,
                    Email = userFound.Email,
                    BirthDate = userFound.BirthDate,
                    CreatedAt = userFound.CreatedAt,
                    CurrentThought = userFound.CurrentThought,
                    Description = userFound.Description,
                    City = userFound.City,
                    State = userFound.State,
                    Role = userFound.Role
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError($"\n AccountController ~ PATCH Index -->  \n {ex}\n");
                return StatusCode(500, $"{ex}");
            }
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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
                var result = await _userManager.CreateAsync(newUser, user.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest($"Erro ao criar usuário: {errors}");
                }
                    
                await _userManager.AddToRoleAsync(newUser, ERoles.Leitor.ToString());

                var roleName = (await _userManager.GetRolesAsync(newUser)).FirstOrDefault();

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
                    Role = new Role
                    {
                        Name = roleName
                    },
                }; 

                return Ok(response);
            }catch(Exception ex)
            {
                _logger.LogError($"\n AccountController ~ Post Index -->  \n {ex}\n");
                return StatusCode(500, $"{ex}");
            }
        }


        /// <summary>
        /// Deleta a conta de um usuário
        /// </summary>
        /// <param name="id">Id da conta que vai ser deletada</param>
        /// <returns>Retorna mensagem de sucesso, apenas status de sucesso.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try {
                if (id == null)
                    return BadRequest("Id de usuário informado inválido.");

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                    return BadRequest("Usuário não existe.");

                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();

                return Ok("A conta foi deletada!");
            }
            catch(Exception ex)
            {
                _logger.LogError($"AccountController ~ Delete  \n {ex}");
                return StatusCode(500, ex.Message );
            }
        }
    }
}
