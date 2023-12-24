using Financas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoOpCartaoController : ControllerBase
    {
        private readonly ITipoOpCartaoRepository _tipoOpCartaoRepository;
            
        public TipoOpCartaoController(ITipoOpCartaoRepository tipoOpCartaoRepository)
        {
            _tipoOpCartaoRepository = tipoOpCartaoRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var tipoOpCartoes = _tipoOpCartaoRepository.Get();
            return Ok(tipoOpCartoes);
        }
    }
}
