using CafezinhoELivrosApi.Data.Models;
using CafezinhoELivrosAPI.Data.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CafezinhoELivrosApi.Controllers;

[ApiController]
[Route("/seed")]
public class SeedController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<SeedController> _logger;

    public SeedController(UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<SeedController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Cria um usuário admin e 3 roles iniciais (Administrador, Leitor, Contribuidor)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> SeedAdminAndRoles()
    {
        try
        {
            await SeedData.CreateRoles(_roleManager, _logger);
            await SeedData.CreateAdmin(_userManager, _roleManager, _logger);

            return Ok("A seed inicial criou admin e roles com sucesso!");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            _logger.LogError(ex, "Erro ao executar a seed inicial para criar admin e roles ");
            return StatusCode(500, "Erro ao executar seed inicial");
        }
    }
}
