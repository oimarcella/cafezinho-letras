using Microsoft.AspNetCore.Mvc;

namespace CafezinhoELivrosApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class TesteController:ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Cafezinho&Letras está rodando!!");
        }
    }
}
