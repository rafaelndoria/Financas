using Financas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Financas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryOpController : ControllerBase
    {
        private readonly ICategoriaOpRepository _categoriaOpRepository;

        public CategoryOpController(ICategoriaOpRepository categoriaOpRepository)
        {
            _categoriaOpRepository = categoriaOpRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categoriasOperacoes = _categoriaOpRepository.Get();
            return Ok(categoriasOperacoes);
        }
    }
}
