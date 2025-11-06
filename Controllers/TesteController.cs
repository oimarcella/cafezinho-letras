using Microsoft.AspNetCore.Mvc;

namespace CafezinhoELivrosApi.Controllers;

[ApiController]
[Route("/")]
public class TesteController:ControllerBase
{
    /// <summary>
    /// Hello world. Apenas para testar se a API está rodando
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API do Cafezinho&Letras está rodando!!");
    }
}
