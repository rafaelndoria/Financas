using Financas.Extensions;
using Financas.Models;
using Financas.Repositories.Interfaces;
using Financas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Financas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly IContaRepository _contaRepository;

        public ContaController(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var contas = _contaRepository.Get();
            return Ok(contas);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBtId(int id)
        {
            var conta = _contaRepository.GetById(id);
            return Ok(conta);
        }

        [HttpGet("GetUsersAccount")]
        [Authorize]
        public IActionResult GetContaUser()
        {
            ClaimsPrincipal usuarioLogado = HttpContext.User;
            var usuarioId = usuarioLogado.Id();

            var contas = _contaRepository.Get(usuarioId);
            return Ok(contas);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ContaViewModel model)
        {
            ClaimsPrincipal usuarioLogado = HttpContext.User;
            var usuarioId = usuarioLogado.Id();

            try
            {
                if (model.Principal == 1)
                {
                    if (_contaRepository.PossuiContaPrincipal(usuarioId))
                        return BadRequest("Usuario pode ter apenas uma conta como ativa");
                }
                   
                var conta = new Conta(model.Nome, model.Principal, (double)model.Balanco, usuarioId);

                if (!_contaRepository.Create(conta))
                    return BadRequest("Nao foi possivel criar a conta");

                return Ok("Conta criada com sucesso");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_contaRepository.Delete(id))
                    return BadRequest("Não foi possivel deletar a conta");

                return Ok("Conta deletada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public IActionResult Update(ContaViewModel model, int id)
        {
            ClaimsPrincipal usuarioLogado = HttpContext.User;
            var usuarioId = usuarioLogado.Id();

            try
            {
                if (!_contaRepository.Update(id, model, usuarioId))
                    return BadRequest("Nao foi possivel atualizar a conta");

                return Ok("Conta Atualizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
