using Financas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoOpController : ControllerBase
    {
        private readonly ITipoOpRepository _tipoOpRepository;

        public TipoOpController(ITipoOpRepository tipoOpRepository)
        {
            _tipoOpRepository = tipoOpRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tipoOperações = _tipoOpRepository.Get();
            return Ok(tipoOperações);
        }
    }
}
